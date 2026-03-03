#!/usr/bin/env node
import { readFileSync, writeFileSync } from 'fs';
import path from 'path';

const inputFile = path.resolve('./public/contract/contract.cse2j');
const outputFile = path.resolve('./src/data/contract.json');

try {
  // Read the .cse2j file
  const rawData = readFileSync(inputFile, 'utf-8');

  // Parse JSON
  const data = JSON.parse(rawData);

  // Write as .json
  writeFileSync(outputFile, JSON.stringify(data, null, 2), 'utf-8');

  console.log(`✅ Successfully converted contract.cse2j → contract.json`);
} catch (err) {
  console.error('❌ Error converting file:', err.message);
  process.exit(1);
}