'use strict';
if (!Sample)
    var Sample = Sample || {};
Sample.Global = Sample.Global || {};
Sample.Global.Resources = null;

Sample.Global.LoadResources = function () {
    var locLang = (navigator.language) ? navigator.language : navigator.userLanguage;
    var url = "/ScriptResx.ashx?culture=" + locLang + "&name=MyResource";
    $.getJSON(url, function (data) {
        Sample.Global.Resources = data;
    });
}

Sample.Global.LoadResources();

//rest of you methods and objects for your solution...
$(function () {
    alert(Sample.Global.Resources.helloWorld); //caution first letter of the resources key will always be in lowercase : json
});