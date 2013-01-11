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

        if (trigger === 'manual') {
            $(elem).tooltip('show');
        }
        if (timeout !== undefined && timeout !== null) {
            setTimeout(function () {
                $(elem).tooltip('hide');
            }, timeout);
        }
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

utils.stringUtils = {

    /**
     * remove trailing whitespaces and line breaks
     */
    trim: function (str) {
        return str.trim().replace(/(\r\n|\n|\r)/gm, " ");
    }
}

utils.printUtils = {
    
    /**
     * prints the given div
     */
    print: function (html) {
        var w = window.open();
        w.document.write(html);
        w.document.close();
        w.focus();
        w.print();
        w.close();
    }
}

utils.fileUtils = {
    
    /**
     * Initializes the jquery-file-upload plugin on the given element
     * and calls the callback whenever a file(s) is selected
     */
    setupFileUpload: function (elem, callback) {
        $(elem).fileupload({
            dataType: 'text',
            url: 'UploadFiles',
            add: function (e, data) {
                callback(data);
            }
        });
    }
}
