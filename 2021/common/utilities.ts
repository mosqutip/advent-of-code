import { readFileSync } from 'fs';

const input_file_name = 'input.txt';
var day = '';

export function readInputFile(file_path: string) {
    try {
        const source_path = file_path.replace('build/', '');
        const source_path_parts = source_path.split('/');
        day = source_path_parts[source_path_parts.length - 1].replace('day', '');
        const data = readFileSync(`${source_path}/${input_file_name}`, 'ascii');
        const lines = data.split('\n');
        if (!lines || !lines.length) {
            throw new Error('Invalid input - check input file!');
        }

        return lines.filter((line) => line !== null && line != '');
    } catch (err) {
        console.error(err);
    }

    return [];
}

export function printAnswer(part: number, answer: string) {
    console.log(`The answer to day ${day}, part ${part} is: ${answer}.`);
}

export function print3DMatrix(matrix: Array<Array<Array<any>>>) {
    for (let i = 0; i < matrix.length; i++) {
        let vals = [];
        for (let j = 0; j < matrix[0].length; j++) {
            vals.push(matrix[i][j][0]);
        }
        console.log(vals.join(''));
    }

    console.log('');
}
