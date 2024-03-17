use std::{io::Write, net::TcpStream};
use crate::utils::{self, dir::DirInfo};

pub mod serialization
{
    use super::{CommandGetFiles, CommandMKDIR, CommandRMDIR, CommandResult, NMStart, NetMessage, NetMessageCallback};
    use std::net::TcpStream;

    pub struct CommandReader
    {
        buffer: Vec<u8>,
        position: usize
    }

    pub struct CommandWriter
    {
        pub buffer: Vec<u8>,
        pub position: usize
    }

    impl CommandWriter {
        pub fn write_i32(&mut self, value: i32)
        {
            let data: [u8; 4] = value.to_ne_bytes();
            for i in 0..4 {
                self.buffer.push(data[i]);
            }
            self.position += 4;
        }

        pub fn write_string_utf8(&mut self, text: String)
        {
            let data: &[u8] = text.as_bytes();
            self.write_i32(data.len() as i32);
            for i in 0..data.len() {
                self.buffer.push(data[i]);
            }
            self.position += data.len();
        }

        pub fn write_bool(&mut self, flag: bool)
        {
            let mut byte = 0;
            if flag 
            {
                byte = 1;
            }
            self.buffer.push(byte);
            self.position += 1;
        }

        pub fn resize(&mut self, )
        {
            let mut i: usize = 0;
            let lenght: usize = self.buffer.len(); 
            while lenght > crate::BUFFER_SIZE * i
            {
                i += 1;
            }
            let need_bytes = i * crate::BUFFER_SIZE;
            let mut space_lenght = need_bytes - lenght;
            if space_lenght < 11
            {
                space_lenght += crate::BUFFER_SIZE;
            }
            let mut space: Vec<u8> = vec![0; space_lenght];
            self.buffer.append(&mut space);
        }
    }

    impl CommandReader {
        pub fn read_i32(&mut self) -> i32
        {
            let mut result: [u8; 4] = [0; 4];
            for i in 0..4 {
                result[i] = self.buffer[self.position + i];
            }
            self.position += 4;
            return i32::from_ne_bytes(result);
        }

        pub fn read_string_utf8(&mut self) -> String
        {
            let lenght: usize = self.read_i32() as usize;
            let result: Vec<u8> = self.buffer[self.position..self.position + lenght].to_vec();
            self.position += lenght;

            return String::from_utf8(result).unwrap();
        }

        pub fn read_bool(&mut self) -> bool
        {
            let byte = self.buffer[self.position];
            self.position += 1;

            return byte != 0;
        }
    }

    pub fn receive_data(from: &mut TcpStream, buffer: Vec<u8>)
    {
        let mut reader = CommandReader { buffer: buffer, position: 0 };
        let struct_id = reader.read_i32();
        match struct_id {
            1 => {
                let mut mes = CommandMKDIR { path: "".to_string() };
                mes.deserialize(&mut reader);
                mes.invoke(from)
            },
            2 => {
                let mut mes = CommandRMDIR { path: "".to_string() };
                mes.deserialize(&mut reader);
                mes.invoke(from)
            },
            3 => {
                let mut mes = CommandGetFiles { path: "".to_string() };
                mes.deserialize(&mut reader);
                mes.invoke(from)
            },
            5 => {
                let mut mes = NMStart { file: "".to_string(), arg: "".to_string() };
                mes.deserialize(&mut reader);
                mes.invoke(from);
            }
            _ => {
                let mut mes = CommandResult { success: false, message: "unknown struct id".to_string() };
                mes.send(from);
            }
        }
    }

}

pub trait NetMessageCallback {
    fn send(&mut self, from: &mut TcpStream)
    {
        let mut writer = serialization::CommandWriter { buffer: Vec::new(), position: 0  };
        self.serialize(&mut writer);
        writer.resize();

        from.write(&writer.buffer).unwrap();
        from.flush().unwrap();
    }
    
    fn serialize(&mut self, writer: &mut serialization::CommandWriter);
}

pub trait NetMessage
{
    fn invoke(&mut self, from: &mut TcpStream);
    fn deserialize(&mut self, reader: &mut serialization::CommandReader);
}

pub struct CommandMKDIR // 1
{
    path: String
}

pub struct CommandRMDIR // 2
{
    path: String
}

pub struct CommandResult // 0
{
    success: bool,
    message: String
}

pub struct CommandGetFiles // 3
{
    path: String
}

pub struct CommandGetFilesResult // 4
{
    data: DirInfo
}

pub struct NMStart // 5
{
    file: String,
    arg: String
}

impl NetMessage for NMStart
{
    fn invoke(&mut self, from: &mut TcpStream) {
        let prc_result = utils::processes::start(self.file.to_string(), self.arg.to_string());
        let mut result: CommandResult;
        if prc_result.is_ok()
        {
            result = CommandResult { success: true, message: "1".to_string() };
        }
        else
        {
            let m = prc_result.err().map(|exc| exc.to_string());
            result = CommandResult { success: false, message: m.unwrap().to_string() };
        }
        result.send(from);
    }

    fn deserialize(&mut self, reader: &mut serialization::CommandReader) {
        self.file = reader.read_string_utf8();
        self.arg = reader.read_string_utf8();
    }
}

impl NetMessage for CommandMKDIR
{

    fn invoke(&mut self, from: &mut TcpStream) {
        if let Err(e) = std::fs::create_dir(self.path.to_string())
        {
            let mut command_result = CommandResult { success: false, message: format!("{}", e) };
            command_result.send(from);
            return;
        }
        let mut command_result = CommandResult { success: true, message: format!("created \"{}\"", self.path.to_string()) };
        command_result.send(from);
    }

    fn deserialize(&mut self, reader: &mut serialization::CommandReader) {
        self.path = reader.read_string_utf8();
    }
}

impl NetMessage for CommandRMDIR
{
    fn invoke(&mut self, from: &mut TcpStream) {
        if let Err(e) = std::fs::remove_dir_all(self.path.to_string())
        {
            let mut command_result = CommandResult { success: false, message: format!("{}", e) };
            command_result.send(from);
            return;
        }
        let mut command_result = CommandResult { success: true, message: format!("deleted \"{}\"", self.path.to_string()) };
        command_result.send(from);
    }

    fn deserialize(&mut self, reader: &mut serialization::CommandReader) {
        self.path = reader.read_string_utf8();
    }
}

impl NetMessageCallback for CommandResult
{
    fn serialize(&mut self, writer: &mut serialization::CommandWriter) {
        writer.write_i32(0); // current type
        writer.write_bool(self.success);
        writer.write_string_utf8(self.message.to_string());
    }
}

impl NetMessageCallback for CommandGetFilesResult
{
    fn serialize(&mut self, writer: &mut serialization::CommandWriter)
    {
        writer.write_i32(4); // id
        writer.write_string_utf8(self.data.path.to_string());
        writer.write_i32(self.data.files.len() as i32);
        for file in &self.data.files
        {
            writer.write_string_utf8(file.file_name.to_string());
            writer.write_bool(file.is_dir);
        }
    }
}

impl NetMessage for CommandGetFiles
{
    fn invoke(&mut self, from: &mut TcpStream) {
        let data = utils::dir::get_files(self.path.to_string());
        let mut dir_info_result = CommandGetFilesResult { data: DirInfo { files: data.files, path: self.path.to_string() } };
        dir_info_result.send(from);
    }

    fn deserialize(&mut self, reader: &mut serialization::CommandReader) {
        self.path = reader.read_string_utf8();
    }
}