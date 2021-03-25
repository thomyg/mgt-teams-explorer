
(function () {
    let loginBtn = document.getElementById("login");
    loginBtn.addEventListener("loginCompleted", () => {
        let helperBtn = document.getElementById("helperBtn");
        helperBtn.click();
    });
})();

window.login = function () {
    if (mgt.Providers.globalProvider._state != 2) {
        mgt.Providers.globalProvider.login();
    }    
}

window.getToken = function () {
    return mgt.Providers.globalProvider.getAccessToken();
}

window.getMGTProviderState = function () {
    if (mgt.Providers.globalProvider._state != null) {
        return mgt.Providers.globalProvider._state;
    }

    return -1;
}

window.getMGTUserInformation = function () {
    var info = new Array();
    info.push(mgt.Providers.globalProvider.userAgentApplication.account.name);
    info.push(mgt.Providers.globalProvider.userAgentApplication.account.userName);
    info.push(mgt.Providers.globalProvider.userAgentApplication.account.accountIdentifier);
    return info;
}


  


