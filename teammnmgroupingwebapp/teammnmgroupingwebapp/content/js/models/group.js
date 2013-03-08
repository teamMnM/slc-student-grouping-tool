define([],
    function () {
        var GroupModel = Backbone.Model.extend({
            defaults: {
                id: '',
                cohortIdentified: '',
                cohortDescription: '',

            }
        });

        return GroupModel;
    }
);