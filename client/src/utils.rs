pub mod dir
{
    pub struct FileInfo
    {
        pub file_name: String,
        pub is_dir: bool,
    }

    pub struct DirInfo
    {
        pub files: Vec<FileInfo>,
        pub path: String
    }

    use std::fs;

    pub fn get_files(path: String) -> DirInfo
    {
        let mut info = DirInfo { path: path.to_string(), files: Vec::new() };

        if let Ok(entries) = fs::read_dir(path) {
            for entry in entries {
                if let Ok(entry) = entry {
                    info.files.push(FileInfo { file_name: entry.file_name().to_string_lossy().into_owned(), is_dir: entry.file_type().unwrap().is_dir() })
                }
            }
        }
        return info;
    }
}