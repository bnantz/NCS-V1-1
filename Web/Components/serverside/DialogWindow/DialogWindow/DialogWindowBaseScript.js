function MetaBuilders_DialogWindow_OpenDialog(url,name,features) {
	if ( window.MetaBuilders_DialogWindow_CurrentDialog != null ) {
		window.MetaBuilders_DialogWindow_CurrentDialog.focus();
	}
	window.MetaBuilders_DialogWindow_CurrentDialogName = name;
	window.MetaBuilders_DialogWindow_CurrentDialog = window.open(url, name, features);
	window.setTimeout(MetaBuilders_DialogWindow_CatchClose,500);
}
function MetaBuilders_DialogWindow_CatchClose() {
	var dialog = MetaBuilders_DialogWindow_CurrentDialog;
	if ( dialog != null ) {
		if ( dialog.closed ) {
			MetaBuilders_DialogWindow_DoDialogPostBack(null);
		} else {
			if ( dialog.screenTop < 0 ) {
				MetaBuilders_DialogWindow_ModalFocus();
			}
			window.setTimeout(MetaBuilders_DialogWindow_CatchClose,500);
		}
	}
}
function MetaBuilders_DialogWindow_ModalFocus(e) {
	var dialog = MetaBuilders_DialogWindow_CurrentDialog;
	if ( dialog != null && !dialog.closed ) {
		dialog.focus();
		if ( window.event ) {
			window.event.cancelBubble = true;
		}
		if ( e && e.stopPropagation ) {
			e.stopPropagation();
		}
		if ( typeof ( dialog.MetaBuilders_DialogWindow_ModalFocus ) != "undefined" ) {
			dialog.MetaBuilders_DialogWindow_ModalFocus(e);
		}
		return false;
	}
	return true;
}
function MetaBuilders_DialogWindow_DoDialogPostBack(result) {
	if ( window.MetaBuilders_DialogWindow_PostingBack ) {
		return;
	}
	window.MetaBuilders_DialogWindow_PostingBack = true;
	window.MetaBuilders_DialogWindow_CurrentDialog = null;
	var theScript = '';
	for( var i = 0; i < MetaBuilders_DialogWindows.length; i = i + 3 ) {
		if ( MetaBuilders_DialogWindows[i] == window.MetaBuilders_DialogWindow_CurrentDialogName ) {
			theScript = MetaBuilders_DialogWindows[i+1];
			if ( result == null ) {
				result = MetaBuilders_DialogWindows[i+2];
			}
		}
	}
	eval( theScript.replace('@dialogResult@',result.replace("'", "\\'")) );
}
function MetaBuilders_DialogWindow_Init() {
	window.MetaBuilders_DialogWindow_CurrentDialog = null;
	window.MetaBuilders_DialogWindow_PostingBack = false;
	if ( typeof( window.addEventListener ) != "undefined" ) {
		window.addEventListener("focus",MetaBuilders_DialogWindow_ModalFocus,true);
		document.addEventListener("mouseover",MetaBuilders_DialogWindow_ModalFocus,true);
	} else if ( typeof( window.attachEvent ) != "undefined" ) {
		window.attachEvent("onfocus",MetaBuilders_DialogWindow_ModalFocus);
		document.attachEvent("onmouseover",MetaBuilders_DialogWindow_ModalFocus);
	} else {
		window.onfocus = MetaBuilders_DialogWindow_ModalFocus;
		document.onmouseover = MetaBuilders_DialogWindow_ModalFocus;
	}
}

