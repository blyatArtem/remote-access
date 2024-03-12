pub mod command_converter
{
    use std::vec;

    pub fn receive_data(code: u32, buffer: &mut [u8; crate::BUFFER_SIZE] )
    {
        let result1 = read_integer(buffer, 0);
        println!("1 message (integer 32): {}", result1.1);

        let result2 = read_integer(buffer, result1.0);
        println!("2 message (text lenght): {}", result2.1);

        let result3 = read_string(buffer, result1.0);
        println!("2 message (text): {}", result3.1);

        let result4 = read_integer(buffer, result3.0);
        println!("3 message (integer 32): {}", result4.1);

    }

    fn read_integer(buffer: &mut [u8; crate::BUFFER_SIZE], index: u32) -> (u32, i32)
    {
        let mut value_buffer: [u8; 4] = [0; 4];
        value_buffer.copy_from_slice(&buffer[index as usize..(index + 4) as usize]);
        let value = i32::from_ne_bytes(value_buffer);
        return (index + 4, value);
    }

    fn read_string(buffer: &mut [u8; crate::BUFFER_SIZE], mut index: u32) -> (u32, String)
    {
        let result: (u32, i32) = read_integer(buffer, index);
        index = result.0;
        let start_index = index;
        let text_lenght = result.1 as u32;

        let mut value_buffer: Vec<u8> = Vec::new();
        while index < start_index + text_lenght
        {
            value_buffer.push(buffer[index as usize]);
            index += 1;
        }
        let value = String::from_utf8(value_buffer).unwrap();
        return  (index, value);
    }
}

// pub trait Command {
//     fn execute(&self);
// }

// impl Command for CreateDir // 1
// {
//     fn execute(&self)
//     {
//         println!("mkdir {}", self.path)
//     }
// }

// impl Command for RemoveDir // 2
// {
//     fn execute(&self)
//     {
//         println!("rmdir {}", self.path)
//     }
// }

// struct CreateDir { path: String }

// struct RemoveDir { path: String }
