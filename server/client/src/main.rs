use std::{io::Write, net::TcpStream};

fn main() {
    let mut stream = TcpStream::connect(format!("{ADDRESS}:{PORT}")).expect("failed to connect");
    stream.set_nonblocking(true).unwrap();
    let mut buffer: [u8; BUFFER_SIZE] = [0; BUFFER_SIZE];
    buffer[0] = 1;
    buffer[1] = 2;
    buffer[2] = 3;
    stream.write(&buffer).unwrap();
}

const ADDRESS: &str = "127.0.0.1";
const PORT: i32 = 228;
const BUFFER_SIZE: usize = 64;