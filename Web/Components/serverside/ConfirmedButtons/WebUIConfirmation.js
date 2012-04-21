var Page_ConfirmationVer = "2.0";

function ConfirmationOnLoad() {
    if ( !ConfirmationBrowserCapable() ) return;
    if (typeof(Page_Confirmations) == "undefined") return;
    var i, confirmButton;
    for (i = 0; i < Page_Confirmations.length; i++) {
        confirmButton = Page_Confirmations[i];
        if (typeof(confirmButton) == "string") {
            confirmButton = document.getElementById( confirmButton );
        }
        if ( typeof(confirmButton.confirmationmessage ) == "undefined" ) {
            if ( typeof( confirmButton.attributes ) != "undefined" ) {
                confirmButton.confirmationmessage = confirmButton.attributes["confirmationmessage"].value;
            } else {
                confirmButton.confirmationmessage = "Are you sure you want to do this?";
            }
        }
        ConfirmationHookupControl(confirmButton);
    }
}

function ConfirmationBrowserCapable() {
    if ( typeof( document.getElementById ) == "undefined" ) {
        if( typeof( document.all ) != "undefined" ) {
            document.getElementById = function( elementId ) { return document.all[elementId]; };
            return true;
        }
        return false;
    }
    return true;
}

function ConfirmationHookupControl( confirmButton ) {
    var ev = confirmButton.onclick;
    if (typeof(ev) == "function" ) {            
        ev = ev.toString();
        ev = ev.substring(ev.indexOf("{") + 1, ev.lastIndexOf("}"));
    }
    else {
        ev = "";
    }
    var func = new Function("if ( !ConfirmationOnClick( this ) ){return false;} " + ev);
    confirmButton.onclick = func;
}

function ConfirmationOnClick( confirmButton ) {
    return confirm( confirmButton.confirmationmessage );
}