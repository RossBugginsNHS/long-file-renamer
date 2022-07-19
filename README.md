# Why needed?
Extracting files from google drive, or elsewhere can give zips with file path names longer than 255 characters.

# Steps
1. Extract a zip using 7zip - this can deal with file names longer than 255, you just wont be able to move them once they are extracted to windows
2. Run tool, with path and max length

# Output
- will rename files, and append ~1, ~2 etc if there are going to be duplicate files after the renaming.
- Leave extension as is
- Does not rename any of the folder path, only the file name.