var group_selection = group_selection || {};

group_selection.student = function(studentData){
	var me = this;
	this.pubSub = PubSub;
	
	this.studentData = studentData;
	this.studentContainerId = null;
		
	this.studentIconClass = ".student-icon";
	this.studentNameClass = ".student-name";
	this.studentAttributesClass = ".student-attributes";
	this.studentFullProfileClass = ".student-full-profile";
	this.studentDelBtnClass = ".del-button";
	this.studentTemplate = '<div class="student-container">' +
								"<img class='del-button' src='/Content/img/student-close-icon.png'></img>" +
								'<div class="student-icon-div">' +
									'<img class="student-icon" src="/Content/img/student-icon-male.png"/>' +
								'</div>' +
								'<div>' +
									"<div class='student-name'></div>" + 
									"<div class='student-attributes'></div>" +
	                                "<div class='student-full-profile'><a href='#'>full profile</a></div>" +
	                            '</div>' + 
                           '</div>';
                           
    /**************************
     * METHODS
     **************************/                      
     this.init = function(){
         me.studentContainerId = "#" + me.studentData.id;

         $(me.studentContainerId).find(me.studentDelBtnClass).click(function (event) {
             me.remove();
         });
     }
     
    /**
     * Return the HTML content for this object 
     */
    this.generateTemplate = function(){
    	var studentData = me.studentData;
    	var template = $(me.studentTemplate);
    	
    	$(template).attr('id', studentData.id);
    	$(template).find(me.studentNameClass).html(studentData.name);
    	
    	var fullProfileLink = $(template).find(me.studentFullProfileClass);
		$(fullProfileLink).click(function (event) {
		    window.open("https://dashboard.sandbox.slcedu.org/s/l/student/" + studentData.id);                         
		});
    	
    	return template;    	
    }
    
    /** 
     * TODO refactor to make lookups generic
     * Show the given attributes on the students in this group
     */
	this.appendStudentAttributes = function(attributes){
		$(me.studentContainerId).find(me.studentAttributesClass).empty();
		var studentData = me.studentData;
	    _.each(attributes, function (attribute) {
	        var name = attribute.attributeName;
	        var value = studentData[attribute.attributeId];
	        if (value === null || value === undefined || value === '' || value.length === 0) {
	            value = '[no data]';
	        }

            // perform lookup for sections
	        if (attribute.attributeId === 'sections') {
	            var sectionNames = [];
	            var studentSections = value;
	            var sections = student_grouping.sections;
	            _.each(studentSections, function (studentSection) {
	                // find the corresponding section using the id
	                var matchingSection = _.find(sections, function (section) {
	                    return section.id === studentSection;
	                });
	                if (matchingSection !== undefined) {
	                    sectionNames.push(matchingSection.courseTitle);
	                }
	            });
	            value = sectionNames;
	        }

			$(me.studentContainerId).find(me.studentAttributesClass).append("<div><strong>" + name + "</strong> " + value + "</div>");
		});
	}

    /**
     * Let others know this student is to be removed
     */
	this.remove = function () {
	    var studentId = me.studentData.id;
	    me.pubSub.publish('remove-student', studentId);

	    $(me.studentContainerId).remove();
	}
}
