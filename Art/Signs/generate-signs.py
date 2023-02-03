#!/usr/bin/env python3

"""
Generate SVGS for all twelve track numbers
"""

trackCount = 12
source = "SignTemplate.svg"

def generateSigns(count):
    for i in range(count):
        filename = f'sign_{i}.svg'
        track_name = f'Track {ii}'
        # Copy file and replace name
        shutil.copy(source, filename)
        with open(filename) as f:
            data = f.read()
        data = data.replace("Track 11", track_name)

        with open(filename, 'w') as file:
            file.write(data)
                    
if __name__ == "__main__":
    generateSigns(trackCount)
