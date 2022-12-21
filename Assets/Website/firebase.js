
// Import the functions you need from the SDKs you need
import { initializeApp } from "https://www.gstatic.com/firebasejs/9.15.0/firebase-app.js";
import { getAuth } from "https://www.gstatic.com/firebasejs/9.15.0/firebase-auth.js";
import { getDatabase, ref, child, get } from "https://www.gstatic.com/firebasejs/9.15.0/firebase-database.js";
// TODO: Add SDKs for Firebase products that you want to use
// https://firebase.google.com/docs/web/setup#available-libraries

// Your web app's Firebase configuration
const firebaseConfig = {
  apiKey: "AIzaSyDhrUkpEd86Ab29x_3_DfH5_pP30XqZ0zA",
  authDomain: "dda-firebase-demo-1647a.firebaseapp.com",
  databaseURL: "https://dda-firebase-demo-1647a-default-rtdb.asia-southeast1.firebasedatabase.app",
  projectId: "dda-firebase-demo-1647a",
  storageBucket: "dda-firebase-demo-1647a.appspot.com",
  messagingSenderId: "805041820636",
  appId: "1:805041820636:web:0a542860b3e4de8ca9b519"
};

// Initialize Firebase
const app = initializeApp(firebaseConfig);

/*
function SelectAllData(){
  firebaseConfig.databaseURL().ref('291').on('value', 
  function(AllRecords){
   AllRecords.forEach(
    function(currentRecord){
      var email = currentRecord.val().email;
      var username = currentRecord.val().username;
      AddItemsToTable(email,username);
    }
   );
  });
}

window.onLoad = SelectAllData;

var tEst = 0
function AddItemsToTable(email,username){
  var tbody = document.getElementById('tbody1');
  var trow = document.createElement('tr');
  var td1 = document.createElement('td');
  var td2 = document.createElement('td');
  var td3 = document.createElement('td'); 
  td1.innerHTML = ++tEst;
  td2.innerHTML = email;
  td3.innerHTML = username;
  trow.appendChild(td1); trow.appendChild(td2);
  tbody.appendChild(trow);
}
*/


const db = getDatabase();
const playerRef = ref(db, "players");
const leaderBoardref = ref(db, "users");
getPlayerData();
function getPlayerData() {
//const playerRef = ref(db, "players");
//PlayerRef is declared at the top using a constant
//get(child(db,players/))
get(playerRef)
.then((snapshot) => {//retrieve a snapshot of the data using a callback
  if (snapshot.exists()) {//if the data exist
    try {
      //let's do something about it
      var content = "";
      snapshot.forEach((childSnapshot) => {//looping through each snapshot
        //https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Ar
        ray / forEach
        console.log("GetPlayerData: childkey " + childSnapshot.key);
      });
    } catch (error) {
      console.log("Error getPlayerData" + error);
    }
  }
});
}//end getPlayerData

const auth = getAuth();
//retrieve element from form
var frmCreateUser = document.getElementById("frmCreateUser");
//we create a button listener to listen when someone clicks
frmCreateUser.addEventListener("submit", function(e) {
  e.preventDefault();
  var email = document.getElementById("email").value;
  var password = document.getElementById("password").value;
  createUser(email, password);
  console.log("email" + email + "password" + password);
});
//create a new user based on email n password into Auth service
//user will get signed in
//userCredential is an object that gets
function createUser(email, password) {
  console.log("creating the user");
  createUserWithEmailAndPassword(auth, email, password)
  .then((userCredential) => {
  //signedin
  const user = userCredential.user;
  console.log("created user ... " + JSON.stringify(userCredential));
  console.log("User is now signed in ");
}).catch((error) => {
  const errorCode = error.code;
  const errorMessage = error.message;
});
}

function getLeaderBoard()
{
  var LeaderBoardEntries = [];
  var entry = [];

  get(leaderBoardref).then((snapshot) =>{
    if(snapshot.exists()){
      try{
        var content = "";
        snapshot.forEach((childSnapshot)=>{
          entry.push(childSnapshot.val().username);
          entry.push(childSnapshot.val().email);
          entry.push(snapshot.key);
          console.log(childSnapshot.val().username);

          LeaderBoardEntries.push(entry)
          entry = []
        });
        console.log(LeaderBoardEntries);
        
        LeaderBoardEntries.sort(function(a,b){
          return a[1] - b[1];
        });

        let rankNum = 1;
        
        let i = LeaderBoardEntries.length - 1;
        console.log(LeaderBoardEntries.length);

        while(i <= 0)
        {
          var onE = `idName${rankNum}`;
          var twO = document.getElementById(onE);
          var thrEE = `idEmail${rankNum}`;
          var foUR = document.getElementById(thrEE);
          twO.innerHTML = LeaderBoardEntries[i][0];
          foUR.innerHTML = LeaderBoardEntries[i][1];
          console.log(LeaderBoardEntries[i]);
          i++;
          rankNum++;


        }
      }
      catch (error)
      {
        console.log("Error getPlayData" + error);
      }
    };
  });
}
getLeaderBoard();