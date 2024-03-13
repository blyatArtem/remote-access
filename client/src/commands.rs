use std::{fmt::Error, intrinsics::r#try, io::Write, net::TcpStream};

use crate::utils;

pub mod command_serializer
{
    use super::{Command, CommandMKDIR, CommandRMDIR};
    use std::net::TcpStream;

    pub struct CommandReader
    {
        buffer: Vec<u8>,
        position: usize
    }

    pub struct CommandWriter
    {
        pub(crate) buffer: Vec<u8>,
        pub(crate) position: usize
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

        pub fn write_string(&mut self, text: String)
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
            while self.buffer.len() > crate::BUFFER_SIZE * i
            {
                i += 1;
            }
            let need_bytes = i * crate::BUFFER_SIZE;
            let mut space: Vec<u8> = vec![0; need_bytes];
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

        pub fn read_string(&mut self) -> String
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
                CommandMKDIR { path: "".to_string() }.invoke(from, &mut reader);
            },
            2 => {
                CommandRMDIR { path: "".to_string() }.invoke(from, &mut reader);
            },
            _ => {
                println!("unknown struct received");
            }
        }
    }

}

pub trait Command {
    fn invoke(&mut self, from: &mut TcpStream, reader: &mut command_serializer::CommandReader)
    {
        self.deserialize(reader);
        self.execute_command(from);
    }
    fn send(&mut self, from: &mut TcpStream)
    {
        let mut writer = command_serializer::CommandWriter { buffer: Vec::new(), position: 0  };
        self.serialize(&mut writer);
        writer.resize();
        from.write(&writer.buffer).unwrap();
    }
    fn deserialize(&mut self, reader: &mut command_serializer::CommandReader);
    fn serialize(&mut self, writer: &mut command_serializer::CommandWriter);
    fn execute_command(&mut self, from: &mut TcpStream);
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

impl Command for CommandMKDIR
{
    fn deserialize(&mut self, reader: &mut command_serializer::CommandReader) {
        self.path = reader.read_string();
    }

    fn execute_command(&mut self, from: &mut TcpStream) {
        if let Err(e) = std::fs::create_dir(self.path.to_string())
        {
            let mut command_result = CommandResult { success: false, message: format!("failed to create \"{}\"", self.path.to_string()) };
            command_result.send(from);
            println!("{}", e);
            return;
        }
        let mut command_result = CommandResult { success: true, message: format!("created \"{}\"", self.path.to_string()) };
        command_result.send(from);
    }

    fn serialize(&mut self, _: &mut command_serializer::CommandWriter) { }
}

impl Command for CommandRMDIR
{
    fn deserialize(&mut self, reader: &mut command_serializer::CommandReader) {
        self.path = reader.read_string();
    }

    fn execute_command(&mut self, from: &mut TcpStream) {
        if let Err(e) = std::fs::remove_dir_all(self.path.to_string())
        {
            let mut command_result = CommandResult { success: false, message: format!("{}", e) };
            command_result.send(from);
            return;
        }
        let mut command_result = CommandResult { success: true, message: format!("deleted \"{}\"", self.path.to_string()) };
        command_result.send(from);
    }

    fn serialize(&mut self, _: &mut command_serializer::CommandWriter) { }
}

impl Command for CommandResult
{
    fn deserialize(&mut self, _: &mut command_serializer::CommandReader) { }

    fn serialize(&mut self, writer: &mut command_serializer::CommandWriter) {
        writer.write_i32(0); // current type
        writer.write_bool(self.success);
        writer.write_string(self.message.to_string());
    }

    fn execute_command(&mut self, _: &mut TcpStream) {
    }
}