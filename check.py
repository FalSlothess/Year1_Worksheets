#! /usr/bin/env python3
##
# Worksheet check script.
# This is provided as a tool to assit students, it is not authorative or mark-breaing.
##

import pathlib

WORKSHEET_ITEMS = {
        'worksheet1': [ '000.user', 'videos.txt' ],
        'worksheet2': [ 'quiz.pdf', 'Program.cs' ],
        'worksheet3': [ 'quiz.pdf', 'Calculator.cs' ],
        'worksheet4': [ 'quiz.pdf', 'Program.cs' ],
        'worksheet5': [ 'quiz.pdf', 'Program.cs' ]
}

def check_worksheet(folder, expected_items):
    """Check the contents of the folder matches the expected submission format"""
    found_items = {}

    for item in expected_items:
        path = folder / item
        found_items[ item ] = path.exists()

    return found_items

def main():
    """Main method"""
    root = pathlib.Path('.')
    for (worksheet, expected) in WORKSHEET_ITEMS.items():

        print("Checking {}...".format( worksheet ))
        result = check_worksheet( root / worksheet, expected )
        for item in expected:
            if result[item]:
                print( "\t {} was found".format( item ) )
            else:
                print( "\t {} is missing!".format( item ) )
        print()

if __name__ == '__main__':
    main()
