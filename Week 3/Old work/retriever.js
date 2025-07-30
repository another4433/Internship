let words = ["apple", "abacus", "academy", "academic study", "advance", "advanced technology", "advanced studies", 
    "addicted", "attractive", "absolute", "asynchronous", "attach", "abstract", "academy for successful future", "banana", 
    "better", "best", "back", "backup", "backwards", "bone", "ball", "beat", "become", "aware", "find", "find the company", 
    "find the problem", "find the solution", "company", "company for better future", "company for building solutions", 
    "company of information technology", "company of academic studies", "company of businesses", "business", 
    "business of information technology solutions"];
const accessor = document.getElementsByTagName("table");
const defaulting = ["apple", "abacus", "academy", "academic study", "advance", "advanced technology", "advanced studies", 
    "addicted", "attractive", "absolute", "asynchronous", "attach", "abstract", "academy for successful future", "banana", 
    "better", "best", "back", "backup", "backwards", "bone", "ball", "beat", "become", "aware", "find", "find the company", 
    "find the problem", "find the solution", "company", "company for better future", "company for building solutions", 
    "company of information technology", "company of academic studies", "company of businesses", "business", 
    "business of information technology solutions"];
let phrases = [];

for (let i = 0; i < accessor.length; i++){
    let row = accessor[i].getElementsByTagName("tr");
    for (let j = 0; j < row.length; j++){
        let column = row[j].getElementsByTagName("td");
        if (column[1].textContent != null){
            words.push(column[1].textContent);
        }
        else {
            words.push(null);
        }
    }
}

function randomize(min, max){
    return Math.floor(Math.random() * (max - min + 1)) + min;
}

for (let a = 0; a < 10; a++){
    let number1 = randomize(0, words.length-1);
    let number2 = randomize(1, words.length-2);
    let text = words[number2]+" "+words[number1];
    words.push(text);
    phrases.push(text);
}

for (let b = 0; b < 20; b++){
    let number1 = randomize(0, words.length-1);
    let number2 = randomize(1, words.length-2);
    let number3 = randomize(2, words.length-3);
    let text = words[number1]+" "+words[number2]+" "+words[number3];
    words.push(text);
    phrases.push(text);
}

for (let c = 0; c < 30; c++){
    let number1 = randomize(0, words.length-1);
    let number2 = randomize(1, words.length-2);
    let number3 = randomize(2, words.length-3);
    let number4 = randomize(3, words.length-4);
    let text = words[number4]+" "+words[number3]+" "+words[number1]+" "+words[number2];
    words.push(text);
    phrases.push(text);
}

console.log(words);
console.log(defaulting);
console.log(phrases);