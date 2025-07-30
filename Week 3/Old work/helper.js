// Get the table element
const table = document.getElementById('myTable');

// Loop through rows (skip header if needed)
for (let i = 0; i < table.rows.length; i++) {
  const row = table.rows[i];
  
  // Loop through cells in each row
  for (let j = 0; j < row.cells.length; j++) {
    const cell = row.cells[j];
    console.log(`Row ${i}, Cell ${j}:`, cell.textContent);
  }
}