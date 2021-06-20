import os

directory = './report/graphs'
files = os.listdir(directory)
for file in files:
    if file[-4:] == '.mmd':
        filename = file[:-4]
        input_file = f'{directory}/{filename}.mmd'
        output_file = f'{directory}/{filename}.png'
        os.system(f'mmdc -i {input_file} -o {output_file}')
