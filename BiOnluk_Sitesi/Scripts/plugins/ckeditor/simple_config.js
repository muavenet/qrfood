CKEDITOR.editorConfig = function (config) {
    config.toolbar = [
		{ name: 'clipboard', items: ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'] },
		{ name: 'editing', items: ['Scayt'] },
		{ name: 'insert', items: ['Table', 'HorizontalRule', 'SpecialChar'] },
		{ name: 'tools', items: ['Maximize'] },
		'/',
		{ name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Strike'] },
		{ name: 'paragraph', items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote'] },
		{ name: 'styles', items: ['Styles', 'Format'] }
    ];
};