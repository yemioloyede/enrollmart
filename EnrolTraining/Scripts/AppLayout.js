    function isValidEmailAddress(emailAddress) {
        var pattern = /^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$/;
        return pattern.test(emailAddress);
    }


function tblsetup(tblid, sort, search, paging, pagesize, ordColumn, ordDirection) {
    $(tblid + ' th').append(" <span></span>");
    if (ordColumn === undefined)
        ordColumn = 0;
    if (ordDirection === undefined)
        ordDirection = 'asc';
    opts = '<"enrtblwrap"tip>';
    if (search || paging)
        opts = '<"enrtblopts"fl><"enrtblwrap"tip>';
    $(tblid).addClass("stripe hover row-border");
    $(tblid).DataTable({
        "stateSave": true,
        "autoWidth": false,
        "order": [ordColumn, ordDirection],
        "paging": paging,
        "searching": search,
        "ordering": sort,
        "columnDefs": [{
            "targets": 'nosort',
            "orderable": false
        }],
        "dom": opts,
        "iDisplayLength": pagesize,
        "stateDuration": -1,
        "oLanguage": {
            "sLengthMenu": "<span class='showentries'>Show:</span> <select>" +
                "<option value='50'>50</option>" +
                "<option value='100'>100</option>" +
                "<option value='-1'>All</option>" +
                "</select>",
            "sZeroRecords": "<b>No Records Found</b>"
        },
        "fnDrawCallback": function () {
            $(this).show();
        }
    });
}

jQuery(document).ready(function () {

    function n(e, t, a) {
        var r = matchMedia(t);
        r.addListener(a),
        r.matches && a(r)
    }
    function s(e) {
        n(e, "(max-width: 767px)", function (t) {
            e.toggleClass("dropdown-menu", t.matches)
        })
    }
    function i(e, t) {
        var a = t.find(".active");
        e.text(a.text())
    }

    function init(e) {
        var t = e.find("ul")
          , a = e.find(".responsive-menu-toggle");
        s(t),
        i(a, t)
    }

    var bc = $(".tabs");
    init(bc);
});

jQuery(document).ready(function () {


    function n(e) {
        e.metisMenu({
            toggle: !0
        })
    }
    function i(e, t) {
        t.find(".navigation-toggle").on("click", function (a) {
            var r = ["expanded", "active", "in"];
            t.find("." + r.join(", .")).removeClass(r.join(" ")),
            e.toggleClass("collapsed-navigation"),
            a.preventDefault()
        })
    }
    function o(e) {
        e.find(".navigation-menu").on("mouseleave", function () {
            $(this).find("li").removeClass("expanded")
        }),
        e.find(".navigation-menu > li").on("hover, click", function (e) {
            e.currentTarget === e.target && ($(this).siblings().removeClass("expanded"),
            $(this).toggleClass("expanded"))
        })
    }
    function u(e, t, a) {
        var r = matchMedia(t);
        r.addListener(a),
        r.matches && a(r)
    }
    function l(e, t) {
        var a = matchMedia("(max-width: 767px)");
        t.find(".navigation-toggle").on("click", function (e) {
            a.matches && t.find(".navigation-menu").slideToggle(),
            e.preventDefault()
        }),
        u(e, "(max-width: 767px)", function (a) {
            var r = t.find(".navigation-menu");
            e.toggleClass("collapsed-navigation", !a.matches),
            s(e),
            a.matches ? r.hide() : r.show(400, function () {
                $(this).removeAttr("style")
            })
        }),
        u(e, "(max-width: 1024px)", function (t) {
            e.toggleClass("collapsed-navigation", t.matches)
        }),
        u(e, "(min-width: 1025px)", function (t) {
            t.matches && (e.removeClass("collapsed-navigation")
            )
        })
    }
    function p(e, t) {
        e.find(".sub-menu ul a").each(function () {
            $(this).parent().addClass("disabled"),
            $(this).replaceWith(this.childNodes)
        })
    }


    function _() {
        $(".navigation-toggle").click(function () {
            $.cookie("collapsed-navigation",
            $(".wrapper").hasClass("collapsed-navigation"))
        })
    }
    function y(e) {
        var t = $.cookie("collapsed-navigation");
        $(".wrapper").toggleClass("collapsed-navigation", "true" === t)
    }

    function init(e) {
        var t = e.find("nav");
        $("html").on("touchstart", function (e) {
            0 === t.has(e.target).length && t.find("li").removeClass("expanded")
        }),
        n(t),
        o(t),
        i(e, t),
        _(),
        y(e)
        
    }

    var t = $(".wrapper");
    init(t);

});


jQuery(document).ready(function () {
    $(".content-box").not(".enrresp").wrap("<div class='content-scroll-wrap'></div>");
    $('tbody.altRows tr:even').addClass("alt-row");
    $('.tipso').tipso();
});