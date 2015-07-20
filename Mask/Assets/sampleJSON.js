#pragma strict

import SimpleJSON;
function Start () {
var s = 
'{"ptype":"snippet",' +
'"text":"I trusted her", '   +
'"type":"statement",' +
'"time":1200,' +
'"emotions":{"anger":0.5 , "disgust":0.6, "happy":0.0, "sad":0.3, "scared":0.0}}';

var packet = JSON.Parse(s);

var packetType = packet["ptype"].Value;
var text = packet["text"].Value;
var type = packet["type"].Value;
var time = packet["time"].AsInt;
var anger = packet["emotions"]["anger"].AsFloat;
var disgust = packet["emotions"]["disgust"].AsFloat;
var happy = packet["emotions"]["happy"].AsFloat;
var sad = packet["emotions"]["sad"].AsFloat;
var scared = packet["emotions"]["scared"].AsFloat;

print ("text:" + text + "\n" + 
"time:" + time + "\n" + 
"anger:" + anger + "\n" +
"disgust:" + disgust + "\n" +
"happy:" + happy + "\n" +
"sad:" + sad + "\n" +
"scared:" + scared + "\n");
}

function Update () {

}