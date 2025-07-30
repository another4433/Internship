let longList = ["apple", "abacus", "academy", "academic study", "advance", "advanced technology", "advanced studies", "addicted", 
"attractive", "absolute", "asynchronous", "attach", "abstract", "academy for successful future", "banana", "better", "best", "back", 
"backup", "backwards", "bone", "ball", "beat", "become", "aware", "find", "find the company", "find the problem", "find the solution", 
"company", "company for better future", "company for building solutions", "company of information technology", 
"company of academic studies", "company of businesses", "business", "business of information technology solutions", "Aberdeen", 
"abolition", "aboriginal", "absolute(ly)", "abstract(s)", "academia, academy, academic(al)", "according to", "account(s)", "accept", 
"acceptance", "accommodation", "accommodate", "accomplished", "accomplish", "accomplishment", "account(s)", "accusative", "accuse",
"accusation", "achievement", "achieve", "achieving", "adaptation (of)", "adapt", "adaptive", "adabat", "company found", "university did",
"hello", "world", "hello world"];

console.log("Started");

function randomize(min, max){
    return Math.floor(Math.random() * (max - min + 1)) + min;
}

for (let a = 0; a < 10; a++){
    let number1 = randomize(0, longList.length-1);
    let number2 = randomize(1, longList.length-2);
    let text = longList[number2]+" "+longList[number1];
    longList.push(text);
}

for (let b = 0; b < 20; b++){
    let number1 = randomize(0, longList.length-1);
    let number2 = randomize(1, longList.length-2);
    let number3 = randomize(2, longList.length-3);
    let text = longList[number1]+" "+longList[number2]+" "+longList[number3];
    longList.push(text);
}

for (let c = 0; c < 30; c++){
    let number1 = randomize(0, longList.length-1);
    let number2 = randomize(1, longList.length-2);
    let number3 = randomize(2, longList.length-3);
    let number4 = randomize(3, longList.length-4);
    let text = longList[number4]+" "+longList[number3]+" "+longList[number1]+" "+longList[number2];
    longList.push(text);
}
const searchInput = document.getElementById("fname");
console.log(textChecker);
const textChecker = searchInput.value;
if (textChecker.length > 0){
    console.log("Entered");
    document.getElementsByTagName("html").innerHTML = `
        <head>
            <title>Google</title>
            <meta content="width=device-width; initial-scale=1.0" name="Resizable page">
        </head>
        <body>
            <nav id="use-of-user" style="font: message-box;">
                <p class="pico" title="Unavailable">Gmail&nbsp;&nbsp;&nbsp;&nbsp;Images</p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <img src="Screenshot 2025-06-16 130754.png" alt="options" class="container" title="Unavailable">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <form action="WIN_20250622_14_53_51_Pro.jpg" target="_self" method="get"><button id="sign-in" title="Profile" type="submit">Sign in</button></form>
                <link rel="stylesheet" href="google.css">
            </nav><br><br><br><br><br><br>
            <main style="font: message-box;">
                <img src="white-hd-google-logo-701751694791445gkogomqqwr.png" alt="Google" id="google-logo" title="Unavailable"><br>
                <div id="searching">
                    <img src="Screenshot 2025-06-16 135113.png" alt="button" id="icon" title="Unavailable">
                    <input type="text" id="fname" name="fname">
                    <img src="Screenshot 2025-06-16 135134.png" alt="button" id="mic" title="Unavailable">
                    <img src="Screenshot 2025-06-16 135150.png" alt="button" id="cam" title="Unavailable">
                </div><br>
                <div id="suggestionBox"></div><br><br>
                <div id="buttons">
                    <form action="WIN_20250622_13_39_17_Pro.jpg" target="_self" method="get"><button title="Google Search" type="submit">Google Search</button>&nbsp;&nbsp;&nbsp;&nbsp;</form>
                    <form action="WIN_20250622_13_37_31_Pro.jpg" target="_self" method="get"><button title="I'm Feeling Lucky" type="submit">I'm Feeling Lucky</button></form>
                </div><br>
                <div id="extra">
                    <p>Google offered in: </p>&nbsp;&nbsp;
                    <a href="https://www.google.com.">العربية</a>
                </div><br><br><br><br><br>
            </main><br><br><br><br>
            <script src="search.js">console.log("The rest are in operation");</script>
        </body><br><br>
        <footer><br>
            <span title="Unavailable" style="font: message-box;"><p id="country">Bahrain</p></span>
            <section id="referencing" style="font: message-box;">
                <nav id="use-of-alternates">
                    <p id="others" title="Unavailable">About&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Advertising&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Business</p>
                    <p id="search-idea" title="Unavailable">How Search works</p>
                </nav>
                <nav id="purposes">
                    <p title="Unavailable">Privacy&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Terms&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Settings</p>
                </nav>
            </section>
        </footer>
    `;
    document.getElementById("use-of-user").style = "margin-left: 83.5%;";
    document.getElementById("sign-in").style = `
        border-radius: 25px;
        background-color: rgb(154, 237, 255);
        width: 85px;
        color: black;
        margin-bottom: 5px;
        margin-top: 5px;
        padding-left: 20px;
        padding-right: 20px;
    `;
    document.getElementsByTagName("html").style = `
        background-color: rgb(34, 34, 34);
        overflow: auto;
        resize: both;
    `;
    document.getElementById("google-logo").style = `
        margin-left: 41%;
        align-items: center;
        align-self: center;
        align-content: center;
        margin-bottom: 1%;
        height: 100px;
    `;
    document.getElementsByTagName("p").style = "color: white; display: flex;";
    document.getElementsByTagName("button").style = `
        word-spacing: 40%;
        border-radius: 5px;
        background-color: rgb(51, 51, 52);
        color: white;
        height: 40px;
        width: 150px;
        border-color: black;
    `;
    document.getElementById("buttons").style = `
        margin-top: 1%;
        margin-left: 41%;
        word-spacing: 30%;
        gap: 0.1px;
    `;
    document.getElementById("extra").style = `
        margin-left: 46%;
        align-items: center;
        align-self: center;
        align-content: center;
    `;
    document.getElementsByTagName("section").style = `
        background-color: black;
        border-top-color: gray;
        border-top-width: 100px;
    `;
    document.getElementById("use-of-alternative").style = `
        margin-right: 40%;
        padding-left: 2rem;
        padding-top: 0.1px;
        padding-bottom: 0.1px;
        margin-bottom: 0.1px;
    `;
    document.getElementById("purposes").style = `
        margin-left: 18%;
        padding-left: 2rem;
        padding-top: 0.1px;
        padding-bottom: 0.1px;
        margin-bottom: 0.1px;
    `;
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
    document.getElementsByTagName("nav").style = "display: flex";
    document.getElementsByTagName("div").style = "display: flex";
    document.getElementById("search-idea").style = `
        margin-left: 13%;
        width: 190px;
    `;
    document.getElementById("others").style = "width: 200px";
    document.getElementById("country").style = `
        background-color: black;
        height: 25px;
        border-bottom-color: gray;
        border-bottom-width: 100px;
        padding-left: 8mm;
        padding-top: 12px;
        padding-bottom: 5px;
        margin-bottom: 4px;
        margin-top: 2px;
    `;
    document.getElementById("mic").style = `
        margin-left: 2%;
        height: 30px;
        margin-right: 3px;
    `;
    document.getElementById("cam").style = `
        margin-left: 10px;
        border-radius: 5mm;
        height: 30px;
        margin-right: 5px;
    `;
    document.getElementById("fname").style = `
        background-color: rgb(78, 79, 80);
        width: 999in;
        margin-left: 10px;
        height: 23px;
        color: white;
        border: none;
        outline: none;
    `;
    document.getElementById("icon").style = `
        border-radius: 10mm;
        height: 30px;
        margin-left: 5px;
    `;
    document.getElementsByClassName("container").style = `
        height: 35px;
        margin-top: 8px;
    `;
    document.getElementById("suggestionBox").style = `
        background-color: rgb(78, 79, 80);
        margin-left: 32%;
        align-items: center;
        align-self: center;
        align-content: center;
        white-space: 20px;
        margin-right: 32%;
        border-bottom-left-radius: 5rem;
        border-bottom-right-radius: 5rem;
    `;
    console.log("Done");
    for (let d = 0; d <= 5; d++){
        console.log(d);
        let number1 = randomize(0, longList.length-1);
        let theString = searchInput.value;
        if (theString.match([longList[number1]])){
            boxEditor.appendChild("<h6 name=\"anItem\">"+longList[number1]+"</h6><br>");
            let boxItem = document.getElementsByName("anItem");
            boxItem.style = `
                background-color: rgb(78, 79, 80);
                margin-left: 32%;
                align-items: center;
                align-self: center;
                align-content: center;
                white-space: 20px;
                margin-right: 32%;
                color: white;
                border-color: white;
                border-top-width: 5rem;
                border-bottom-width: 5rem;
            `;
            console.log("Executed");
        }
        else {
            d = d - 1;
        }
    }
}
console.log("The end");