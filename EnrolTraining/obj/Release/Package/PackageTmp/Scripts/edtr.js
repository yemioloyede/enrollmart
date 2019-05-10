tinyMCE.init({
    mode: "specific_textareas",
    editor_selector: "cmsEditor",
    plugins: [
        "advlist autolink lists link image charmap print preview anchor textcolor",
        "searchreplace visualblocks code fullscreen",
        "insertdatetime media table contextmenu paste"
    ],
    menu: {
        edit: { title: 'Edit', items: 'undo redo | cut copy paste pastetext | selectall' },
        insert: { title: 'Insert', items: 'link image' },
        view: { title: 'View', items: 'visualaid' },
        format: { title: 'Format', items: 'bold italic underline strikethrough superscript subscript | formats | removeformat' },
        table: { title: 'Table', items: 'inserttable tableprops deletetable | cell row column' },
        tools: { title: 'Tools', items: 'code' }
    },
    style_formats_merge: true,
    style_formats: [
      {
          title: 'Image Left',
          selector: 'img',
          styles: {
              'float': 'left',
              'margin': '0 10px 0 10px'
          }
      },
       {
           title: 'Image Right',
           selector: 'img',
           styles: {
               'float': 'right',
               'margin': '0 0 10px 10px'
           }
       }
    ],

    toolbar: "undo redo | bold italic forecolor fontsizeselect | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image",
    content_css: "/admin/assets/enr/css/tinymce.css?ver=1",
    relative_urls: false,
    convert_urls: false
});

tinyMCE.init({
    mode: "specific_textareas",
    editor_selector: "clientEditor",
    plugins: [
        "advlist autolink lists link image charmap print preview anchor textcolor",
        "searchreplace visualblocks code fullscreen",
        "insertdatetime media table contextmenu paste"
    ],
    menu: {
        edit: { title: 'Edit', items: 'undo redo | cut copy paste pastetext | selectall' },
        insert: { title: 'Insert', items: 'link image' },
        view: { title: 'View', items: 'visualaid' },
        format: { title: 'Format', items: 'bold italic underline strikethrough superscript subscript | formats | removeformat' },
        table: { title: 'Table', items: 'inserttable tableprops deletetable | cell row column' },
        tools: { title: 'Tools', items: 'code' }
    },
    //style_formats_merge: true,
    style_formats: [
      {
          title: 'Image Left',
          selector: 'img',
          styles: {
              'float': 'left',
              'margin': '0 10px 0 10px'
          }
      },
       {
           title: 'Image Right',
           selector: 'img',
           styles: {
               'float': 'right',
               'margin': '0 0 10px 10px'
           }
       }
    ],

    toolbar: "undo redo | bold italic forecolor fontsizeselect | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image",
    file_browser_callback: FileManager,
    //content_css: "/Content/Site.css",
    relative_urls: false,
    convert_urls: false
});

function RoxyFileBrowser(field_name, url, type, win) {
    var roxyFileman = '/index.html';
    if (roxyFileman.indexOf("?") < 0) {
        roxyFileman += "?type=" + type;
    }
    else {
        roxyFileman += "&type=" + type;
    }
    roxyFileman += '&field_name=' + field_name + '&value=' + win.document.getElementById(field_name).value;
    if (tinyMCE.activeEditor.settings.language) {
        roxyFileman += '&langCode=' + tinyMCE.activeEditor.settings.language;
    }
    tinyMCE.activeEditor.windowManager.open({
        file: roxyFileman,
        title: 'File Manager',
        width: 850,
        height: 650,
        resizable: "yes",
        plugins: "media",
        inline: "yes",
        close_previous: "no"
    }, { window: win, input: field_name });
    return false;
}

function FileManager(field_name, url, type, win) {
		    
    var w = window,
    d = document,
    e = d.documentElement,
    g = d.getElementsByTagName('body')[0],
    x = w.innerWidth || e.clientWidth || g.clientWidth,
    y = w.innerHeight|| e.clientHeight|| g.clientHeight;

    var cmsURL = '/index.html?&field_name='+field_name+'&langCode=en';
		    
    if(type == 'image') {		    
        cmsURL = cmsURL + "&type=images";
    }
		
    tinyMCE.activeEditor.windowManager.open({
        file : cmsURL,
        title : 'Filemanager',
        width : x * 0.8,
        height : y * 0.8,
        resizable : "yes",
        close_previous : "no"
    });		    

}