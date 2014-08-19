'use strict';
if (!Sample)
    var Sample = Sample || {};
Sample.Global = Sample.Global || {};
Sample.Global.Resources = null;

Sample.Global.LoadResources = function () {
    var context = new SP.ClientContext.get_current();
    this.website = context.get_web();
    context.load(this.website, 'Title', 'ServerRelativeUrl');
    context.executeQueryAsync(Function.createDelegate(this, Sample.Global.LoadUserInfoSuccess), Function.createDelegate(this, Sample.Global.LoadUserInfoFailed));

}
Sample.Global.LoadUserInfoSuccess = function (sender, args) {
    var locLang = (navigator.language) ? navigator.language : navigator.userLanguage;
    var serverRelativeUrl = this.website.get_serverRelativeUrl();
    if (serverRelativeUrl.length < 2) { serverRelativeUrl = ""; }
    var url = serverRelativeUrl + "/_layouts/ScriptResx.ashx?culture=" + locLang + "&name=MyResource";
    sp$.getScript(url, function (script, status) {
        Sample.Global.Resources = Res;
        Res = null;
    });
    this.website = null;
}
Sample.Global.LoadUserInfoFailed = function (sender, args) {
    //should do something?
}

Sample.Global.LoadResources();

//rest of you methods and objects for your solution...
sp$(function () {
    alert(Sample.Global.Resources.helloWorld); //caution first letter of the resources key will always be in lowercase : json
});