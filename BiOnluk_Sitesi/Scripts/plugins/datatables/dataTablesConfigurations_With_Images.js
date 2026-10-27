var $dataTables = jQuery.noConflict();

$dataTables(document).ready(function () {

    $dataTables('#tblListe').dataTable({

        "order": [],

        "language": {
            "sProcessing": "İşleniyor...",
            "sLengthMenu": "Sayfada _MENU_ Kayıt Göster",
            "sZeroRecords": "Eşleşen Kayıt Bulunmadı",
            "sInfo": "  _TOTAL_ Kayıttan _START_ - _END_ Arası Kayıtlar",
            "sInfoEmpty": "Kayıt Yok",
            "sInfoFiltered": "( _MAX_ Kayıt İçerisinden Bulunan)",
            "sInfoPostFix": "",
			"sSearch": "",
			"sSearchPlaceholder": "Aranacak kelime...",
            "sUrl": "",
            "oPaginate": {
                "sFirst": "İlk",
                "sPrevious": "Önceki",
                "sNext": "Sonraki",
                "sLast": "Son"
            }
        },


        "aLengthMenu": [[20, 30, 50], [20, 30, 50]],
        "bPaginate": true,
        "bLengthChange": true,
        "bFilter": true,
        "bSort": true,
        "bInfo": true,
		"bAutoWidth": true,
		"destroy": true

    });




});