function MetaBuilders_FirstFocus_Focus( id, name)  {
	var targetControl = MetaBuilders_FirstFocus_FindControl( id, name );
	if ( targetControl != null && targetControl.focus ) {
		targetControl.focus();
	}
}
function MetaBuilders_FirstFocus_FindControl( id, name ) {
	if ( typeof(document.getElementById) != "undefined" ) {
		var focusControl = document.getElementById(id);
		if ( focusControl != null ) {
			return focusControl;
		}
	}
	for( var i = 0; i < document.forms.length; i++ ) {
		var theForm = document.forms[i];
		var focusControl = theForm[name];
		if ( focusControl != null ) {
			return focusControl;
		}
	}
	return null;
}