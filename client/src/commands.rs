pub mod command_serializer
{
    use super::{Command, CommandMKDIR};

    pub struct CommandReader
    {
        buffer: Vec<u8>,
        position: usize
    }

    pub struct CommandWriter
    {
        buffer: Vec<u8>,
        position: usize
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
            let mut result: Vec<u8> = self.buffer[self.position..self.position + lenght].to_vec();
            self.position += lenght;

            return String::from_utf8(result).unwrap();
        }
    }

    pub fn receive_data(code: u32, buffer: Vec<u8> )
    {
        let mut reader = CommandReader { buffer: buffer, position: 0 };
        let struct_id = reader.read_i32();

        let mut cmd_mkdir: CommandMKDIR = CommandMKDIR { path: "".to_string() };
        
        cmd_mkdir.try_invoke(struct_id, &mut reader);
    }
}

pub trait Command {
    fn try_invoke(&mut self, id: i32, reader: &mut command_serializer::CommandReader)
    {
        if self.get_id() == id
        {
            self.deserialize(reader);
            self.execute_command();
        }
    }
    fn deserialize(&mut self, reader: &mut command_serializer::CommandReader);
    fn get_id(&self) -> i32;
    fn execute_command(&self);
}

pub struct CommandMKDIR
{
    path: String
}

pub struct CommandRMDIR
{
    path: String
}

impl Command for CommandMKDIR
{
    fn get_id(&self) -> i32 {
        return 1;
    }

    fn deserialize(&mut self, reader: &mut command_serializer::CommandReader) {
        self.path = reader.read_string();
    }

    fn execute_command(&self) {
        println!("mkdir: {}", self.path);
    }
}

impl Command for CommandRMDIR
{
    fn get_id(&self) -> i32 {
        return 2;
    }

    fn deserialize(&mut self, reader: &mut command_serializer::CommandReader) {
        self.path = reader.read_string();
    }

    fn execute_command(&self) {
        println!("rmdir: {}", self.path);
    }
}