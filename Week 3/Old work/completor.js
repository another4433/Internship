let longList = ["apple", "abacus", "academy", "academic study", "advance", "advanced technology", "advanced studies", "addicted", 
"attractive", "absolute", "asynchronous", "attach", "abstract", "academy for successful future", "banana", "better", "best", "back", 
"backup", "backwards", "bone", "ball", "beat", "become", "aware", "find", "find the company", "find the problem", "find the solution", 
"company", "company for better future", "company for building solutions", "company of information technology", 
"company of academic studies", "company of businesses", "business", "business of information technology solutions", "Aberdeen", 
"abolition", "aboriginal", "absolute(ly)", "abstract(s)", "academia, academy, academic(al)", "according to", "account(s)", "accept", 
"acceptance", "accommodation", "accommodate", "accomplished", "accomplish", "accomplishment", "account(s)", "accusative", "accuse",
"accusation", "achievement", "achieve", "achieving", "adaptation (of)", "adapt", "adaptive", "adabat", "company found", "university did",
"hello", "world", "hello world", "research", "calculus", "chemistry", "physics", "computer", "program", "application", "code", 
"addition(s)", "address", "adjective, adjectival", "phrase", "admiral", "administration", "administrative", "advanced", "debug", "bug",
"error(s)", "advances", "advance", "adventure(s)", "advice(s)", "advocate", "advancement(s)", "advertisement(s)", "advertise", 
"aerodynamics", "affair(s)", "affecting", "affect", "affection(s)", "effect(s)", "cause(s)", "agriculture", "agricultural", "africa(n)",
"alchemy", "against", "algebra", "geometry", "information", "system", "software", "engineer(s)", "engineering", "cloud", "compute", 
"finder", "alphabet", "alphabetical", "Ali", "Mohamed", "Redha", "Hassan", "Hussain", "Ahmed", "Bader", "Ebrahim", "America", 
"American", "analysis", "analytic(al)", "anatomy", "anatomical", "ancient", "anecdotes", "Anglian", "Indian", "Anglo", "Europe", "Asia",
"animal(s)", "annual", "anniversary", "annotation", "Anonymous", "answer", "antiquities", "anthropology", "anthropologic(al)", 
"antiquity", "antiquities", "apology", "apologies", "apologize", "apparently", "appendix", "applied", "before", "christ", "british", 
"britain", "columbia", "before", "bell", "dragon", "belgian", "between", "bibliography", "biography", "autobiography", "biochemical",
"biochemist", "biochemistry", "chemical", "chemistry", "chemist", "science", "scientist", "scientific", "calculator", "capture", 
"biographic(al)", "autobiographic(al)", "biology", "biological", "book(s)", "national", "corps", "corpus", "border", "church", 
"masget", "prophet", "imam", "Eman", "Layla", "Lily", "bishop", "brazil", "brazilian", "building", "bullet", "in", "bulletin", "bureau",
"bounty", "bound", "bridge", "century", "centuries", "county", "counties", "syria", "jordan", "palestine", "iraq", "iran", "qatar", "oman"];

let generator = [];

function randomize(min, max){
    return Math.floor(Math.random() * (max - min + 1)) + min;
}

for (let c = 0; c < 50; c++){
    let number1 = randomize(0, longList.length-1);
    let number2 = randomize(1, longList.length-2);
    let number3 = randomize(2, longList.length-3);
    let number4 = randomize(3, longList.length-4);
    let text = longList[number4]+" "+longList[number3]+" "+longList[number1]+" "+longList[number2];
    if (text.length < 50)
        generator.push(text);
    else 
        generator.push(longList[number1]+" "+longList[number4]);
}

for (let b = 0; b < 50; b++){
    let number1 = randomize(0, longList.length-1);
    let number2 = randomize(1, longList.length-2);
    let number3 = randomize(2, longList.length-3);
    let text = longList[number3]+" "+longList[number1]+" "+longList[number2];
    if (text.length < 50){
        longList.push(text);
        generator.push(text);
    }
    else {
        longList.push(longList[number1]+" "+longList[number3]);
        generator.push(longList[number1]+" "+longList[number3]);
    }
}

for (let a = 0; a < 25; a++){
    let number1 = randomize(0, longList.length-1);
    let number2 = randomize(0, generator.length-1);
    let text = generator[number2]+" "+longList[number1];
    longList.push(text);
}

function leastMatch(bigString, smallString){
    const bigSize = bigString.length;
    const smallSize = smallString.length;
    let counter = 0;
    let flag = false;
    for (let i = 0; i < (bigSize-smallSize)+1; i++){
        for (let j = 0; j < smallSize; j++){
            if (bigString[i+j] == smallString[j]){
                counter = counter + 1;
                if (counter >= smallSize){
                    flag = true;
                    break;
                }
            }
            else {
                break;
            }
        }
        if (counter < smallSize){
            counter = 0;
        }
        if (flag){
            break;
        }
    }
    return (counter >= smallSize);
}

const searchInput = document.getElementById("fname");
searchInput.addEventListener('input', (e) => {
    const textChecker = e.target.value;
    console.log("Input: "+textChecker);
    const boxEditor = document.getElementById("suggestionBox");
    if (textChecker.length > 0){
        document.getElementById("searching").style.borderBottomLeftRadius = "0%";
        document.getElementById("searching").style.borderBottomRightRadius = "0%";
        boxEditor.style = `
            background-color: rgb(78, 79, 80);
            margin-left: 32%;
            align-items: start;
            align-self: flex-start;
            align-content: start;
            white-space: 20px;
            margin-right: 32%;
            height: 300px;
            display: flex;
            flex-direction: column;
            line-height: 1px;
            padding-top: 10px;
            border-top: 1px solid black;
        `;
        boxEditor.style.borderRadius = "";
        console.info("Results found: ");
        boxEditor.innerHTML = "";
        let counting = 0;
        for (let d = 0; d <= 5; d++){
            let number1 = randomize(0, longList.length-1);
            let theString = searchInput.value;
            let myString = longList[number1];
            if (leastMatch(myString, theString)){
                console.log(myString);
                //boxEditor.appendChild(`<h6 name="anItem">${myString}</h6><br>`);
                boxEditor.innerHTML += `<h6 class="anItem">${myString}</h6>`;
                if (myString.length > 30){
                    document.getElementsByClassName("anItem").style = "height: 35px";
                    document.getElementsByTagName("h6").style = "height: 35px";
                }
                counting = counting + 1;
            }
        }
        if (counting !== 0){
            document.getElementById("searching").style.borderTopLeftRadius = "2rem";
            document.getElementById("searching").style.borderTopRightRadius = "2rem";
            //boxEditor.style.borderBottomLeftRadius = "0rem";
            //boxEditor.style.borderBottomRightRadius = "0rem";
        }
        switch(counting){
            case 0: 
                document.getElementById("suggestionBox").style = ""; 
                document.getElementById("searching").style.borderBottomLeftRadius = "";
                document.getElementById("searching").style.borderBottomRightRadius = "";
                document.getElementById("searching").style.borderRadius = "5rem";
                break;
            case 1: document.getElementById("suggestionBox").style.height = "60px"; break;
            case 2: document.getElementById("suggestionBox").style.height = "100px"; break;
            case 3: document.getElementById("suggestionBox").style.height = "150px"; break;
            case 4: document.getElementById("suggestionBox").style.height = "200px"; break;
            case 5: document.getElementById("suggestionBox").style.height = "240px"; break;
            default: document.getElementById("suggestionBox").style.height = "290px";
        }
    }
    else {
        boxEditor.style = "";
        document.getElementById("searching").style = `
            background-color: rgb(78, 79, 80);
            margin-left: 32%;
            align-items: center;
            align-self: center;
            align-content: center;
            white-space: 20px;
            margin-right: 32%;
            border-radius: 5rem;
            padding-top: 8px;
            padding-bottom: 8px;
            padding-left: 5px;
            padding-right: 5px;
        `;
        boxEditor.innerHTML = "";
    }
});