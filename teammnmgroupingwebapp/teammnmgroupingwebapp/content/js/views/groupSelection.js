define(["mustache!templates/userInactivityModal.hbs"],
    function (userInactivityModalTemplate) {
        var GroupSelectionView = Backbone.View.extend({
            el: $('#group-selection'),
            render: function () {                
                var html = userInactivityModalTemplate();
                this.$el.append(html);
                return this.$el;
            }
        });

        return GroupSelectionView;
    }
);