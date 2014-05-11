var switcher = {
    beginEdit: function (id) {
        $('#form-' + id).children('.iinput').each(function (i, e) {
            $(e).val($('#' + e.id.substr(2)).text());
        });

        this.toggle(id);
    },

    endEdit: function (id, copy) {
        if (copy) {
            $('#static-' + id).children('.itext').each(function (i, e) {
                $(e).text($('#f_' + e.id).val());
            });
        }

        this.toggle(id);
    },

    toggle: function (id) {
        $('#form-' + id).toggle();
        $('#static-' + id).toggle();
    }
}