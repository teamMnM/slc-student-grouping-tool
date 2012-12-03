var utils = utils || {};

utils.arrayUtils = {
	
	shuffle : function(array) {
	    var tmp, current, top = array.length;
	
	    if(top) while(--top) {
	        current = Math.floor(Math.random() * (top + 1));
	        tmp = array[current];
	        array[current] = array[top];
	        array[top] = tmp;
	    }
	
	    return array;
	}
}

utils.uiUtils = {

    /**
     * Create a tooltip on the fly for the given element     
     */
    showTooltip: function (elem, msg, placement, trigger, timeout) {
        // create tooltip on the fly
        $(elem).tooltip('destroy');
        $(elem).tooltip({
            title: msg,
            placement: placement,
            trigger: trigger
        });

        $(elem).tooltip('show');
        setTimeout(function () {
            $(elem).tooltip('hide');
        }, timeout);
    }, 

    getUrlParams: function () {
        var params = {};
        window.location.search.replace(/[?&]+([^=&]+)=([^&]*)/gi, function(str,key,value) {
            params[key] = value;
        });
 
        return params;
    },
    
    /**
     * Returns true if the given text is empty or blank text 
     */
    textIsEmpty: function (text) {
        return !/\S/.test(text);
    }
}
