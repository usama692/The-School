var TableEditable = function ()
{
    return {

        //main function to initiate the module
        init: function ()
        {
            if ($("table[data-dataTable='true']").length >= 1)
            {
                $("table[data-dataTable='true']").dataTable(
                    {
                        "aLengthMenu": [[5, 10, 20, 25, -1], [5, 10, 20, 25, "All"]]
                    });
            }
            return;
        }
    };
}();