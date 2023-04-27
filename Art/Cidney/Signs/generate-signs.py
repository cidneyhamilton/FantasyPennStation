#!/usr/bin/env python3

"""
Generate SVGS for all twelve track numbers
"""
import shutil
from cairosvg import svg2png

trackCount = 16
source = "SignTemplate.svg"

def generateSigns(count):
    for i in range(1,count+1):
        filename = f'sign_{i}.svg'
        pngname = f'sign_{i}.png'
        track_name = f'Track {i}'
        # Copy file and replace name
        shutil.copy(source, filename)
        with open(filename) as f:
            data = f.read()
        data = data.replace("Track 11", track_name)

        with open(filename, 'w') as file:
            file.write(data)

        svg2png(bytestring=data,write_to=pngname)
                    
if __name__ == "__main__":
    generateSigns(trackCount)
