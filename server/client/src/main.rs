use std::{io::{Read, Write}, net::TcpStream};

mod commands;

fn main() {
    let mut stream = TcpStream::connect(format!("{ADDRESS}:{PORT}")).expect("failed to connect");
    // stream.set_nonblocking(true).unwrap();
    let mut buffer: [u8; BUFFER_SIZE] = [0; BUFFER_SIZE];
    loop {
        stream.read_exact(&mut buffer).unwrap();
        commands::command_converter::receive_data(0, &mut buffer);
    }
}

const ADDRESS: &str = "127.0.0.1";
const PORT: i32 = 228;
const BUFFER_SIZE: usize = 1024;