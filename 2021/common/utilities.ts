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
