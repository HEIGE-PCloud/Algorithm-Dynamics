import os
import argparse

parser = argparse.ArgumentParser(description='Build graphs')
parser.add_argument('--command',
                    dest='command',
                    default='./node_modules/.bin/mmdc',
                    type=str,
                    help='Command for mermaid-cli')
parser.add_argument('--directory',
                    dest='directory',
                    default='./report/graphs',
                    type=str,
                    help='Directory of the graphs')
args = parser.parse_args()

directory = args.directory
command = args.command
files = os.listdir(directory)
for file in files:
    if file[-4:] == '.mmd':
        filename = file[:-4]
        input_file = f'{directory}/{filename}.mmd'
        output_file = f'{directory}/{filename}.png'
        print(f'{command} -i {input_file} -o {output_file}')
        os.system(f'{command} -i {input_file} -o {output_file}')
