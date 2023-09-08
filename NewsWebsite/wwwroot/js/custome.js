
var editorsArray = [];
var editors = document.querySelectorAll(".editor");
var config = {
    skin: 'v2',
    licenseKey: '',

};

$(".editor").ckeditor(config);


//$('.editor').ckeditor();
//if (editors.length) {
//    $.getScript("../lib/ckeditor/ckeditor.js",
//        function (data, textStatus, jqxhr) {
//            for (editor of editors) {
//                ClassicEditor
//                    .create(editor,
//                        {
//                            licenseKey: '',
//                            simpleUpload: {
//                                uploadUrl: '/Home/UploadEditorImage'
//                            }
//                        })
//                    .then(editor => {
//                        window.editor = editor;
//                        editorsArray.push(editor);
//                    })
//                    .catch(error => {
//                        console.log(error);
//                    });
//            }
//        });
//}
