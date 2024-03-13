use std::{io::Read, net::TcpStream};

mod commands;
mod utils;

fn main() {
    let mut stream = TcpStream::connect(format!("{ADDRESS}:{PORT}")).expect("failed to connect");
    loop {
        let mut buffer: Vec<u8> = Vec::new();
        loop 
        {
            let mut read_buffer: [u8; BUFFER_SIZE] = [0; BUFFER_SIZE];
            stream.read_exact(&mut read_buffer).unwrap();
            buffer.append(&mut read_buffer.to_vec());
            if !continue_reading(read_buffer)
            {
                break;
            }
        }
        commands::command_serializer::receive_data(&mut stream, buffer);
    }
}

fn continue_reading(read_buffer: [u8; BUFFER_SIZE]) -> bool
{
    let lenght = read_buffer.len();
    for i in 1..11 {
        if read_buffer[lenght - i] != 0
        {
            return true;
        }
    }
    return  false;
}

const ADDRESS: &str = "127.0.0.1";
const PORT: i32 = 228;
const BUFFER_SIZE: usize = 32;