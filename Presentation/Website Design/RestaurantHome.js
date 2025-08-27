const theFirst = document.getElementById("first");
const theLast = document.getElementById("last");
const theView = document.getElementById("view");
const theProfiling = document.getElementById("theProfiling");
const theAdder = document.getElementById("Add");
theFirst.addEventListener('click', () => {
    window.location.assign("RestaurantFirstOrder.html");
});
theLast.addEventListener('click', () => {
    window.location.assign("RestaurantLastOrder.html");
});
theView.addEventListener('click', () => {
    window.location.assign("RestaurantSearch.html");
});
theProfiling.addEventListener('click', () => {
    window.location.assign("RestaurantProfile.html");
});
theAdder.addEventListener('click', () => {
    window.location.assign("RestaurantAddItem.html");
});