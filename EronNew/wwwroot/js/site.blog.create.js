$(function () {
    //$("#demoCustomPalette").mdbWYSIWYG();
    Quill.prototype.getHtml = function () {
        return this.container.querySelector('.ql-editor').innerHTML;
    };

    $(".btn-publish").click(function () {
        alert(Quill.getHtml());
    });

    var toolbarOptions = [
        ['bold', 'italic', 'underline', 'strike'],        // toggled buttons
        ['blockquote', 'code-block'],

        [{ 'header': 1 }, { 'header': 2 }],               // custom button values
        [{ 'list': 'ordered' }, { 'list': 'bullet' }],
        [{ 'script': 'sub' }, { 'script': 'super' }],      // superscript/subscript
        [{ 'indent': '-1' }, { 'indent': '+1' }],          // outdent/indent
        [{ 'direction': 'rtl' }],                         // text direction

        [{ 'size': ['small', false, 'large', 'huge'] }],  // custom dropdown
        [{ 'header': [1, 2, 3, 4, 5, 6, false] }],

        [{ 'color': [] }, { 'background': [] }],          // dropdown with defaults from theme
        [{ 'font': [] }],
        [{ 'align': [] }],
        ['link', 'image'],
        ['code-block'],
        ['clean'],
        [{ 'html': 'HTML' }]


    ];

    var options = {
        debug: 'error',
        syntax: true,
        modules: {
            toolbar: toolbarOptions
        },
        placeholder: 'Compose an epic...',
        theme: 'snow'
    };
    var quill = new Quill('#editor-container', options);

    quill.keyboard.addBinding({
        key: 'B',
        shortKey: true
    }, function (range, context) {
        this.quill.formatText(range, 'bold', true);
    });

    // addBinding may also be called with one parameter,
    // in the same form as in initialization
    quill.keyboard.addBinding({
        key: 'B',
        shortKey: true,
        handler: function (range, context) {

        }
    });


    var quillEd_txtArea_1 = document.createElement('textarea');
    var attrQuillTxtArea = document.createAttribute('quill__html');
    quillEd_txtArea_1.setAttributeNode(attrQuillTxtArea);

    var quillCustomDiv = quill.addContainer('ql-custom');
    quillCustomDiv.appendChild(quillEd_txtArea_1);

    var quillsHtmlBtns = document.querySelectorAll('.ql-html');
    for (var i = 0; i < quillsHtmlBtns.length; i++) {
        quillsHtmlBtns[i].addEventListener('click', function (evt) {
            var wasActiveTxtArea_1 =
                (quillEd_txtArea_1.getAttribute('quill__html').indexOf('-active-') > -1);

            if (wasActiveTxtArea_1) {
                //html editor to quill
                quill.pasteHTML(quillEd_txtArea_1.value);
                evt.target.classList.remove('ql-active');
            } else {
                //quill to html editor
                quillEd_txtArea_1.value = quill.getHtml();
                evt.target.classList.add('ql-active');
            }

            quillEd_txtArea_1.setAttribute('quill__html', wasActiveTxtArea_1 ? '' : '-active-');
        });
    }
});

//translations: {
//        paragraph: 'P\u00e1rrafo',
//        heading: 'Encabezado',
//        preformatted: 'Preformateado',
//        bold: 'Negrita',
//        italic: 'It\u00e1lica',
//        strikethrough: 'Tachado',
//        underline: 'Subrayado',
//        textcolor: 'Color del texto',
//        alignleft: 'Alinear a la izquierda',
//        aligncenter: 'Alinear al centro',
//        alignright: 'Alinear a la derecha',
//        alignjustify: 'Justificar',
//        insertlink: 'Insertar enlace',
//        insertpicture: 'Insertar imagen',
//        bulletlist: 'Lista de vi\u00f1etas',
//        numberedlist: 'Lista numerada',
//        enterurl: 'Insertar enlace',
//        imageurl: 'Insertar enlace de imagen',
//        linkdescription: 'Descripci\u00f3n de la enlace',
//        linkname: 'El nombre del enlace',
//        showHTML: 'C\u00f3digo de HTML'
//      }