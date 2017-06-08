/*
 * Flexigrid for jQuery -  v1.1
 *
 * Copyright (c) 2008 Paulo P. Marinas (code.google.com/p/flexigrid/)
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 * removeData() 清除数据
 */
(function ($) {
    /*
	 * jQuery 1.9 support. browser object has been removed in 1.9 
	 */
    var browser = $.browser

    if (!browser) {
        function uaMatch(ua) {
            ua = ua.toLowerCase();

            var match = /(chrome)[ \/]([\w.]+)/.exec(ua) ||
				/(webkit)[ \/]([\w.]+)/.exec(ua) ||
				/(opera)(?:.*version|)[ \/]([\w.]+)/.exec(ua) ||
				/(msie) ([\w.]+)/.exec(ua) ||
				ua.indexOf("compatible") < 0 && /(mozilla)(?:.*? rv:([\w.]+)|)/.exec(ua) ||
				[];

            return {
                browser: match[1] || "",
                version: match[2] || "0"
            };
        };

        var matched = uaMatch(navigator.userAgent);
        browser = {};

        if (matched.browser) {
            browser[matched.browser] = true;
            browser.version = matched.version;
        }

        // Chrome is Webkit, but Webkit is also Safari.
        if (browser.chrome) {
            browser.webkit = true;
        } else if (browser.webkit) {
            browser.safari = true;
        }
    }

    /*!
     * START code from jQuery UI
     *
     * Copyright 2011, AUTHORS.txt (http://jqueryui.com/about)
     * Dual licensed under the MIT or GPL Version 2 licenses.
     * http://jquery.org/license
     *
     * http://docs.jquery.com/UI
     */

    if (typeof $.support.selectstart != 'function') {
        $.support.selectstart = "onselectstart" in document.createElement("div");
    }

    if (typeof $.fn.disableSelection != 'function') {
        $.fn.disableSelection = function () {
            return this.bind(($.support.selectstart ? "selectstart" : "mousedown") +
                ".ui-disableSelection", function (event) {
                    event.preventDefault();
                });
        };
    }

    //var selectedItem = [];//记录选中行----------------------------------------------------------edit by zj 2013-10-9

    /* END code from jQuery UI */

    $.addFlex = function (t, p) {
        if (t.grid) return false; //return if already exist
        p = $.extend({ //apply default properties
            height: 200, //default height
            width: 'auto', //auto width
            striped: true, //apply odd even stripes
            novstripe: false,
            minwidth: 30, //min width of columns
            minheight: 80, //min height of columns
            resizable: true, //allow table resizing
            url: false, //URL if using data from AJAX
            method: 'POST', //data sending method
            dataType: 'xml', //type of data for AJAX, either xml or json
            errormsg: 'Connection Error',
            usepager: false,
            nowrap: true,
            page: 1, //current page
            total: 1, //total pages
            useRp: true, //use the results per page select box
            rp: 15, //results per page
            rpOptions: [10, 15, 20, 30, 50, 1000], //allowed per-page values
            title: false,
            idProperty: 'id',
            pagestat: 'Displaying {from} to {to} of {total} items',
            pagetext: 'Page',
            outof: 'of',
            findtext: 'Find',
            params: [], //allow optional parameters to be passed around
            procmsg: 'Processing, please wait ...',
            query: '',
            qtype: '',
            nomsg: 'No items',
            minColToggle: 1, //minimum allowed column to be hidden
            showToggleBtn: false, //show or hide column toggle popup
            hideOnSubmit: true,
            autoload: true,
            blockOpacity: 0.5,
            preProcess: false,
            addTitleToCell: false, // add a title attr to cells with truncated contents
            dblClickResize: false, //auto resize column by double clicking
            onChangeSort: false,
            onDoubleClick: false,
            onSuccess: false,
            onError: false,
            onSubmit: false, //using a custom populate function
            __mw: { //extendable middleware function holding object
                datacol: function (p, col, val) { //middleware for formatting data columns
                    var _col = (typeof p.datacol[col] == 'function') ? p.datacol[col](val) : val; //format column using function
                    if (typeof p.datacol['*'] == 'function') { //if wildcard function exists
                        return p.datacol['*'](_col); //run wildcard function
                    } else {
                        return _col; //return column without wildcard
                    }
                }
            },
            getGridClass: function (g) { //get the grid class, always returns g
                return g;
            },
            datacol: {}, //datacol middleware object 'colkey': function(colval) {}
            colResize: true, //from: http://stackoverflow.com/a/10615589
            colMove: true,
            selectedItem: []    // 将selectedItem数组移至DOM对象内
        }, p);
        $(t).show() //show if hidden
			.attr({
			    cellPadding: 0,
			    cellSpacing: 0,
			    border: 0
			}) //remove padding and spacing
			.removeAttr('width'); //remove width properties
        //create grid class
        var g = {
            hset: {},
            addData: function (data) { //parse data
                if (p.dataType == 'json') {
                    data = $.extend({ rows: [], page: 0, total: 0 }, data);
                }
                if (p.preProcess) {
                    data = p.preProcess(data);
                }
                $('.pReload', this.pDiv).removeClass('loading');
                this.loading = false;
                if (!data) {
                    $('.pPageStat', this.pDiv).html(p.errormsg);
                    if (p.onSuccess) p.onSuccess(this);
                    return false;
                }
                if (p.dataType == 'xml') {
                    p.total = +$('rows total', data).text();
                } else {
                    p.total = data.total;
                }
                if (p.total === 0) {
                    $('tr, a, td, div', t).unbind();
                    $("tbody", t).empty();
                    p.pages = 1;
                    p.page = 1;
                    this.buildpager();
                    $('.pPageStat', this.pDiv).html(p.nomsg);
                    if (p.onSuccess) p.onSuccess(this);
                    return false;
                }
                p.pages = Math.ceil(p.total / p.rp);
                if (p.dataType == 'xml') {
                    p.page = +$('rows page', data).text();
                } else {
                    p.page = data.page;
                }
                this.buildpager();
                //build new body
                var tbody = document.createElement('tbody');
                if (p.dataType == 'json') {
                    $.each(data.rows, function (i, row) {
                        var tr = document.createElement('tr');
                        $(tr).click(function () {//添加点击事件，用于记录选中行数据-------------------------------------------------edit by zj 2013-10-9
                            if (p.singleSelect) {//若为单选
                                p.selectedItem = [];
                                p.selectedItem.push(row);
                            } else {
                                var _contain = p.selectedItem.indexOf(row);
                                if (_contain != -1) {
                                    p.selectedItem.splice(_contain, 1);
                                } else {
                                    p.selectedItem.push(row);
                                }
                            }
                        });
                        if (row.name) tr.name = row.name;
                        if (row.color) {
                            $(tr).css('background', row.color);
                        } else {
                            if (i % 2 && p.striped) tr.className = 'erow';
                        }
                        if (row[p.idProperty]) {
                            tr.id = 'row' + row[p.idProperty];
                        }
                        $('thead tr:first th', t).each( //add cell
							function () {
							    var td = document.createElement('td');
							    var idx = $(this).attr('axis').substr(3);
							    td.align = this.align;
							    // If each row is the object itself (no 'cell' key)
							    if (typeof row.cell == 'undefined') {
							        //添加render方法，处理显示结果 render(/*当前单元格的数据*/cell,/*当前行数据*/row,/*所有数据*/data,/*行号*/index)--------------------------------edit by zj 2013-10-9
							        var _col = p.colModel[idx];
							        var _cell = row[_col.name];
							        if (_col.render) {
							            td.innerHTML = _col.render(_cell, row, data, i);
							        } else {
							            td.innerHTML = _cell;
							        }
							    } else {
							        // If the json elements aren't named (which is typical), use numeric order
							        var iHTML = '';
							        if (typeof row.cell[idx] != "undefined") {
							            iHTML = (row.cell[idx] !== null) ? row.cell[idx] : ''; //null-check for Opera-browser
							        } else {
							            iHTML = row.cell[p.colModel[idx].name];
							        }
							        td.innerHTML = p.__mw.datacol(p, $(this).attr('abbr'), iHTML); //use middleware datacol to format cols
							    }
							    // If the content has a <BGCOLOR=nnnnnn> option, decode it.
							    var offs = td.innerHTML.indexOf('<BGCOLOR=');
							    if (offs > 0) {
							        $(td).css('background', text.substr(offs + 7, 7));
							    }

							    $(td).attr('abbr', $(this).attr('abbr'));
							    $(tr).append(td);
							    td = null;
							}
						);
                        if ($('thead', this.gDiv).length < 1) {//handle if grid has no headers
                            for (idx = 0; idx < row.cell.length; idx++) {
                                var td = document.createElement('td');
                                // If the json elements aren't named (which is typical), use numeric order
                                if (typeof row.cell[idx] != "undefined") {
                                    td.innerHTML = (row.cell[idx] != null) ? row.cell[idx] : '';//null-check for Opera-browser
                                } else {
                                    td.innerHTML = row.cell[p.colModel[idx].name];
                                }
                                $(tr).append(td);
                                td = null;
                            }
                        }
                        $(tbody).append(tr);
                        tr = null;
                    });
                } else if (p.dataType == 'xml') {
                    var i = 1;
                    $("rows row", data).each(function () {
                        i++;
                        var tr = document.createElement('tr');
                        if ($(this).attr('name')) tr.name = $(this).attr('name');
                        if ($(this).attr('color')) {
                            $(tr).css('background', $(this).attr('id'));
                        } else {
                            if (i % 2 && p.striped) tr.className = 'erow';
                        }
                        var nid = $(this).attr('id');
                        if (nid) {
                            tr.id = 'row' + nid;
                        }
                        nid = null;
                        var robj = this;
                        $('thead tr:first th', t).each(function () {
                            var td = document.createElement('td');
                            var idx = $(this).attr('axis').substr(3);
                            td.align = this.align;

                            var text = $("cell:eq(" + idx + ")", robj).text();
                            var offs = text.indexOf('<BGCOLOR=');
                            if (offs > 0) {
                                $(td).css('background', text.substr(offs + 7, 7));
                            }
                            td.innerHTML = p.__mw.datacol(p, $(this).attr('abbr'), text); //use middleware datacol to format cols
                            $(td).attr('abbr', $(this).attr('abbr'));
                            $(tr).append(td);
                            td = null;
                        });
                        if ($('thead', this.gDiv).length < 1) {//handle if grid has no headers
                            $('cell', this).each(function () {
                                var td = document.createElement('td');
                                td.innerHTML = $(this).text();
                                $(tr).append(td);
                                td = null;
                            });
                        }
                        $(tbody).append(tr);
                        tr = null;
                        robj = null;
                    });
                }
                $('tbody tr', t).unbind();
                $('tbody', t).remove();
                $(t).append(tbody);
                this.addCellProp();
                this.addRowProp();
                tbody = null;
                data = null;
                i = null;
                if (p.onSuccess) {
                    p.onSuccess(this);
                }
                if (p.hideOnSubmit) {
                    $(g.block).remove();
                }
                if (browser.opera) {
                    $(t).css('visibility', 'visible');
                }
            },
            changeSort: function (th) { //change sortorder
                if (this.loading) {
                    return true;
                }
                $(g.nDiv).hide();
                $(g.nBtn).hide();
                if (p.sortname == $(th).attr('abbr')) {
                    if (p.sortorder == 'asc') {
                        p.sortorder = 'desc';
                    } else {
                        p.sortorder = 'asc';
                    }
                }
                $(th).addClass('sorted').siblings().removeClass('sorted');
                $('.sdesc', t).removeClass('sdesc');
                $('.sasc', t).removeClass('sasc');
                $('div', th).addClass('s' + p.sortorder);
                p.sortname = $(th).attr('abbr');
                if (p.onChangeSort) {
                    p.onChangeSort(p.sortname, p.sortorder);
                } else {
                    this.populate();
                }
            },
            buildpager: function () { //rebuild pager based on new properties
                $('.pcontrol input', this.pDiv).val(p.page);
                $('.pcontrol span', this.pDiv).html(p.pages);
                var r1 = (p.page - 1) * p.rp + 1;
                var r2 = r1 + p.rp - 1;
                if (p.total < r2) {
                    r2 = p.total;
                }
                var stat = p.pagestat;
                stat = stat.replace(/{from}/, r1);
                stat = stat.replace(/{to}/, r2);
                stat = stat.replace(/{total}/, p.total);
                $('.pPageStat', this.pDiv).html(stat);
            },
            populate: function () { //get latest data
                if (this.loading) {
                    return true;
                }
                if (p.onSubmit) {
                    var gh = p.onSubmit();
                    if (!gh) {
                        return false;
                    }
                }
                this.loading = true;
                if (!p.url) {
                    return false;
                }
                $('.pPageStat', this.pDiv).html(p.procmsg);
                $('.pReload', this.pDiv).addClass('loading');
                $(g.block).css({
                    top: g.bDiv.offsetTop
                });
                if (p.hideOnSubmit) {
                    $(this.gDiv).prepend(g.block);
                }
                if (browser.opera) {
                    $(t).css('visibility', 'hidden');
                }
                if (!p.newp) {
                    p.newp = 1;
                }
                if (p.page > p.pages) {
                    p.page = p.pages;
                }
                var param = [{
                    name: 'page',
                    value: p.newp
                }, {
                    name: 'rp',
                    value: p.rp
                }, {
                    name: 'sortname',
                    value: p.sortname
                }, {
                    name: 'sortorder',
                    value: p.sortorder
                }, {
                    name: 'query',
                    value: p.query
                }, {
                    name: 'qtype',
                    value: p.qtype
                }];
                if (p.params.length) {
                    for (var pi = 0; pi < p.params.length; pi++) {
                        param[param.length] = p.params[pi];
                    }
                }
                $.ajax({
                    type: p.method,
                    url: p.url,
                    data: param,
                    dataType: p.dataType,
                    success: function (data) {
                        g.addData(data);
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        try {
                            if (p.onError) p.onError(XMLHttpRequest, textStatus, errorThrown);
                        } catch (e) { }
                    }
                });
            },
            changePage: function (ctype) { //change page
                if (this.loading) {
                    return true;
                }
                switch (ctype) {
                    case 'first':
                        p.newp = 1;
                        break;
                    case 'prev':
                        if (p.page > 1) {
                            p.newp = parseInt(p.page, 10) - 1;
                        }
                        break;
                    case 'next':
                        if (p.page < p.pages) {
                            p.newp = parseInt(p.page, 10) + 1;
                        }
                        break;
                    case 'last':
                        p.newp = p.pages;
                        break;
                    case 'input':
                        var nv = parseInt($('.pcontrol input', this.pDiv).val(), 10);
                        if (isNaN(nv)) {
                            nv = 1;
                        }
                        if (nv < 1) {
                            nv = 1;
                        } else if (nv > p.pages) {
                            nv = p.pages;
                        }
                        $('.pcontrol input', this.pDiv).val(nv);
                        p.newp = nv;
                        break;
                }
                if (p.newp == p.page) {
                    return false;
                }
                if (p.onChangePage) {
                    p.onChangePage(p.newp);
                } else {
                    this.populate();
                }
            },
            addCellProp: function () {
                $('tbody tr td', g.bDiv).each(function () {
                    var tdDiv = document.createElement('div');
                    var n = $('td', $(this).parent()).index(this);
                    var pth = $('th:eq(' + n + ')', t).get(0);
                    if (pth != null) {
                        if (p.sortname == $(pth).attr('abbr') && p.sortname) {
                            this.className = 'sorted';
                        }
                        $(tdDiv).css({
                            textAlign: pth.align,
                            width: $('div:first', pth)[0].style.width
                        });
                        if (pth.hidden) {
                            $(this).css('display', 'none');
                        }
                    }
                    if (p.nowrap == false) {
                        $(tdDiv).css('white-space', 'normal');
                    }
                    if (this.innerHTML == '') {
                        this.innerHTML = '&nbsp;';
                    }
                    tdDiv.innerHTML = this.innerHTML;
                    var prnt = $(this).parent()[0];
                    var pid = false;
                    if (prnt.id) {
                        pid = prnt.id.substr(3);
                    }
                    if (pth != null) {
                        if (pth.process) pth.process(tdDiv, pid);
                    }
                    $(this).empty().append(tdDiv).removeAttr('width'); //wrap content
                    g.addTitleToCell(tdDiv);
                });
            },
            getCellDim: function (obj) {// get cell prop for editable event
                var ht = parseInt($(obj).height(), 10);
                var pht = parseInt($(obj).parent().height(), 10);
                var wt = parseInt(obj.style.width, 10);
                var pwt = parseInt($(obj).parent().width(), 10);
                var top = obj.offsetParent.offsetTop;
                var left = obj.offsetParent.offsetLeft;
                var pdl = parseInt($(obj).css('paddingLeft'), 10);
                var pdt = parseInt($(obj).css('paddingTop'), 10);
                return {
                    ht: ht,
                    wt: wt,
                    top: top,
                    left: left,
                    pdl: pdl,
                    pdt: pdt,
                    pht: pht,
                    pwt: pwt
                };
            },
            addRowProp: function () {
                $('tbody tr', g.bDiv).on('click', function (e) {
                    var obj = (e.target || e.srcElement);
                    if (obj.href || obj.type) return true;
                    if (e.ctrlKey || e.metaKey) {
                        // mousedown already took care of this case
                        return;
                    }
                    $(this).toggleClass('trSelected');
                    if (p.singleSelect && !g.multisel) {
                        $(this).siblings().removeClass('trSelected');
                    }
                }).on('mousedown', function (e) {
                    if (e.shiftKey) {
                        $(this).toggleClass('trSelected');
                        g.multisel = true;
                        this.focus();
                        $(g.gDiv).noSelect();
                    }
                    if (e.ctrlKey || e.metaKey) {
                        $(this).toggleClass('trSelected');
                        g.multisel = true;
                        this.focus();
                    }
                }).on('mouseup', function (e) {
                    if (g.multisel && !(e.ctrlKey || e.metaKey)) {
                        g.multisel = false;
                        $(g.gDiv).noSelect(false);
                    }
                }).on('dblclick', function () {
                    if (p.onDoubleClick) {
                        p.onDoubleClick(this, g, p);
                    }
                }).hover(function (e) {
                    if (g.multisel && e.shiftKey) {
                        $(this).toggleClass('trSelected');
                    }
                }, function () { });
                if (browser.msie && browser.version < 7.0) {
                    $(this).hover(function () {
                        $(this).addClass('trOver');
                    }, function () {
                        $(this).removeClass('trOver');
                    });
                }
            },

            combo_flag: true,
            combo_resetIndex: function (selObj) {
                if (this.combo_flag) {
                    selObj.selectedIndex = 0;
                }
                this.combo_flag = true;
            },
            combo_doSelectAction: function (selObj) {
                eval(selObj.options[selObj.selectedIndex].value);
                selObj.selectedIndex = 0;
                this.combo_flag = false;
            },
            //Add title attribute to div if cell contents is truncated
            addTitleToCell: function (tdDiv) {
                if (p.addTitleToCell) {
                    var $span = $('<span />').css('display', 'none'),
						$div = (tdDiv instanceof jQuery) ? tdDiv : $(tdDiv),
						div_w = $div.outerWidth(),
						span_w = 0;

                    $('body').children(':first').before($span);
                    $span.html($div.html());
                    $span.css('font-size', '' + $div.css('font-size'));
                    $span.css('padding-left', '' + $div.css('padding-left'));
                    span_w = $span.innerWidth();
                    $span.remove();

                    if (span_w > div_w) {
                        $div.attr('title', $div.text());
                    } else {
                        $div.removeAttr('title');
                    }
                }
            },
            pager: 0
        };

        g = p.getGridClass(g); //get the grid class

        if (p.colModel) { //create model if any
            thead = document.createElement('thead');
            var tr = document.createElement('tr');
            for (var i = 0; i < p.colModel.length; i++) {
                var cm = p.colModel[i];
                var th = document.createElement('th');
                $(th).attr('axis', 'col' + i);
                if (cm) {	// only use cm if its defined
                    if ($.cookies) {
                        var cookie_width = 'flexiwidths/' + cm.name;		// Re-Store the widths in the cookies
                        if ($.cookie(cookie_width) != undefined) {
                            cm.width = $.cookie(cookie_width);
                        }
                    }
                    if (cm.display != undefined) {
                        th.innerHTML = cm.display;
                    }
                    if (cm.name && cm.sortable) {
                        $(th).attr('abbr', cm.name);
                    }
                    if (cm.align) {
                        th.align = cm.align;
                    }
                    if (cm.width) {
                        $(th).attr('width', cm.width);
                    }
                    if ($(cm).attr('hide')) {
                        th.hidden = true;
                    }
                    if (cm.process) {
                        th.process = cm.process;
                    }
                } else {
                    th.innerHTML = "";
                    $(th).attr('width', 30);
                }
                $(tr).append(th);
            }
            $(thead).append(tr);
            $(t).prepend(thead);
        } // end if p.colmodel
        //init divs
        g.gDiv = document.createElement('div'); //create global container
        g.mDiv = document.createElement('div'); //create title container
        //g.hDiv = document.createElement('div'); //create header container
        g.bDiv = document.createElement('div'); //create body container
        g.vDiv = document.createElement('div'); //create grip
        g.rDiv = document.createElement('div'); //create horizontal resizer
        //g.cDrag = document.createElement('div'); //create column drag
        g.block = document.createElement('div'); //creat blocker
        //g.nDiv = document.createElement('div'); //create column show/hide popup
        //g.nBtn = document.createElement('div'); //create column show/hide button
        g.iDiv = document.createElement('div'); //create editable layer
        g.tDiv = document.createElement('div'); //create toolbar
        g.sDiv = document.createElement('div');
        g.pDiv = document.createElement('div'); //create pager container

        //if (p.colResize === false) { //don't display column drag if we are not using it
        //    $(g.cDrag).css('display', 'none');
        //}

        if (!p.usepager) {
            g.pDiv.style.display = 'none';
        }
        g.gDiv.className = 'flexigrid';
        if (p.width != 'auto') {
            g.gDiv.style.width = p.width + (isNaN(p.width) ? '' : 'px');
        }
        //add conditional classes
        if (browser.msie) {
            $(g.gDiv).addClass('ie');
        }
        if (p.novstripe) {
            $(g.gDiv).addClass('novstripe');
        }
        $(t).before(g.gDiv);
        $(g.gDiv).append(t);

        // Define a combo button set with custom action'ed calls when clicked.
        if (p.combobuttons && $(g.tDiv2)) {
            var btnDiv = document.createElement('div');
            btnDiv.className = 'fbutton';

            var tSelect = document.createElement('select');
            $(tSelect).change(function () { g.combo_doSelectAction(tSelect) });
            $(tSelect).click(function () { g.combo_resetIndex(tSelect) });
            tSelect.className = 'cselect';
            $(btnDiv).append(tSelect);

            for (i = 0; i < p.combobuttons.length; i++) {
                var btn = p.combobuttons[i];
                if (!btn.separator) {
                    var btnOpt = document.createElement('option');
                    btnOpt.innerHTML = btn.name;

                    if (btn.bclass)
                        $(btnOpt)
						.addClass(btn.bclass)
						.css({ paddingLeft: 20 })
                    ;
                    if (btn.bimage)  // if bimage defined, use its string as an image url for this buttons style (RS)
                        $(btnOpt).css('background', 'url(' + btn.bimage + ') no-repeat center left');
                    $(btnOpt).css('paddingLeft', 20);

                    if (btn.tooltip) // add title if exists (RS)
                        $(btnOpt)[0].title = btn.tooltip;

                    if (btn.onpress) {
                        btnOpt.value = btn.onpress;
                    }
                    $(tSelect).append(btnOpt);
                }
            }
            $('.tDiv2').append(btnDiv);
        }

        var thead = $("thead:first", t).get(0);
        if (thead) $(t).append(thead);
        thead = null;
        if (!p.colmodel) var ci = 0;
        $('thead tr:first th', t).each(function () {
            var thdiv = document.createElement('div');
            if ($(this).attr('abbr')) {
                $(this).click(function (e) {
                    if (!$(this).hasClass('thOver')) return false;
                    var obj = (e.target || e.srcElement);
                    if (obj.href || obj.type) return true;
                    g.changeSort(this);
                });
                if ($(this).attr('abbr') == p.sortname) {
                    this.className = 'sorted';
                    thdiv.className = 's' + p.sortorder;
                }
            }
            if (this.hidden) {
                $(this).hide();
            }
            if (!p.colmodel) {
                $(this).attr('axis', 'col' + ci++);
            }

            // if there isn't a default width, then the column headers don't match
            // i'm sure there is a better way, but this at least stops it failing
            if (this.width == '') {
                this.width = 100;
            }

            $(thdiv).css({
                textAlign: this.align//,
                //width: this.width + 'px'
            });
            thdiv.innerHTML = this.innerHTML;
            $(this).empty().append(thdiv).removeAttr('width').hover(function () {
                if (!g.colresize && !$(this).hasClass('thMove') && !g.colCopy) {
                    $(this).addClass('thOver');
                }
                if ($(this).attr('abbr') != p.sortname && !g.colCopy && !g.colresize && $(this).attr('abbr')) {
                    $('div', this).addClass('s' + p.sortorder);
                } else if ($(this).attr('abbr') == p.sortname && !g.colCopy && !g.colresize && $(this).attr('abbr')) {
                    var no = (p.sortorder == 'asc') ? 'desc' : 'asc';
                    $('div', this).removeClass('s' + p.sortorder).addClass('s' + no);
                }
                if (g.colCopy) {
                    var n = $('thead th', t).index(this);
                    if (n == g.dcoln) {
                        return false;
                    }
                    if (n < g.dcoln) {
                        $(this).append(g.cdropleft);
                    } else {
                        $(this).append(g.cdropright);
                    }
                    g.dcolt = n;
                }
            }, function () {
                $(this).removeClass('thOver');
                if ($(this).attr('abbr') != p.sortname) {
                    $('div', this).removeClass('s' + p.sortorder);
                } else if ($(this).attr('abbr') == p.sortname) {
                    var no = (p.sortorder == 'asc') ? 'desc' : 'asc';
                    $('div', this).addClass('s' + p.sortorder).removeClass('s' + no);
                }
                if (g.colCopy) {
                    $(g.cdropleft).remove();
                    $(g.cdropright).remove();
                    g.dcolt = null;
                }
            }); //wrap content
        });
        //set bDiv
        g.bDiv.className = 'bDiv';
        $(t).before(g.bDiv);
        $(g.bDiv).css({
            height: (p.height == 'auto') ? 'auto' : p.height + "px",
            overflow: 'hidden'
        }).append(t);
        if (p.height == 'auto') {
            $('table', g.bDiv).addClass('autoht');
        }
        //add td & row properties
        g.addCellProp();
        g.addRowProp();
        //add strip
        if (p.striped) {
            $('tbody tr:odd', g.bDiv).addClass('erow');
        }
        // add pager
        if (p.usepager) {
            g.pDiv.className = 'pDiv';
            g.pDiv.innerHTML = '<div class="pDiv2"></div>';
            $(g.bDiv).after(g.pDiv);
            var html = ' <div class="pGroup"> <div class="pFirst pButton"><span></span></div><div class="pPrev pButton"><span></span></div> </div> <div class="btnseparator"></div> <div class="pGroup"><span class="pcontrol">' + p.pagetext + ' <input type="text" size="4" value="1" /> ' + p.outof + ' <span> 1 </span></span></div> <div class="btnseparator"></div> <div class="pGroup"> <div class="pNext pButton"><span></span></div><div class="pLast pButton"><span></span></div> </div> <div class="btnseparator"></div> <div class="pGroup"> <div class="pReload pButton"><span></span></div> </div> <div class="btnseparator"></div> <div class="pGroup"><span class="pPageStat"></span></div>';
            $('div', g.pDiv).html(html);
            $('.pReload', g.pDiv).click(function () {
                g.populate();
            });
            $('.pFirst', g.pDiv).click(function () {
                g.changePage('first');
            });
            $('.pPrev', g.pDiv).click(function () {
                g.changePage('prev');
            });
            $('.pNext', g.pDiv).click(function () {
                g.changePage('next');
            });
            $('.pLast', g.pDiv).click(function () {
                g.changePage('last');
            });
            $('.pcontrol input', g.pDiv).keydown(function (e) {
                if (e.keyCode == 13) {
                    g.changePage('input');
                }
            });
            if (browser.msie && browser.version < 7) $('.pButton', g.pDiv).hover(function () {
                $(this).addClass('pBtnOver');
            }, function () {
                $(this).removeClass('pBtnOver');
            });
            if (p.useRp) {
                var opt = '',
					sel = '';
                for (var nx = 0; nx < p.rpOptions.length; nx++) {
                    if (p.rp == p.rpOptions[nx]) sel = 'selected="selected"';
                    else sel = '';
                    opt += "<option value='" + p.rpOptions[nx] + "' " + sel + " >" + p.rpOptions[nx] + "&nbsp;&nbsp;</option>";
                }
                $('.pDiv2', g.pDiv).prepend("<div class='pGroup'><select name='rp'>" + opt + "</select></div> <div class='btnseparator'></div>");
                $('select', g.pDiv).change(function () {
                    if (p.onRpChange) {
                        p.onRpChange(+this.value);
                    } else {
                        p.newp = 1;
                        p.rp = +this.value;
                        g.populate();
                    }
                });
            }
        }
        $(g.pDiv, g.sDiv).append("<div style='clear:both'></div>");
        // add title
        if (p.title) {
            g.mDiv.className = 'mDiv';
            g.mDiv.innerHTML = '<div class="ftitle">' + p.title + '</div>';
            $(g.gDiv).prepend(g.mDiv);
            if (p.showTableToggleBtn) {
                $(g.mDiv).append('<div class="ptogtitle" title="Minimize/Maximize Table"><span></span></div>');
                $('div.ptogtitle', g.mDiv).click(function () {
                    $(g.gDiv).toggleClass('hideBody');
                    $(this).toggleClass('vsble');
                });
            }
        }
        //setup cdrops
        g.cdropleft = document.createElement('span');
        g.cdropleft.className = 'cdropleft';
        g.cdropright = document.createElement('span');
        g.cdropright.className = 'cdropright';
        //add block
        g.block.className = 'gBlock';
        var gh = $(g.bDiv).height();
        var gtop = g.bDiv.offsetTop;
        $(g.block).css({
            width: g.bDiv.style.width,
            height: gh,
            background: 'white',
            position: 'relative',
            marginBottom: (gh * -1),
            zIndex: 1,
            top: gtop,
            left: '0px'
        });
        $(g.block).fadeTo(0, p.blockOpacity);
        // add date edit layer
        $(g.iDiv).addClass('iDiv').css({
            display: 'none'
        });
        $(g.bDiv).append(g.iDiv);
        // add flexigrid events
        $(g.bDiv).hover(function () {
            $(g.nDiv).hide();
            $(g.nBtn).hide();
        }, function () {
            if (g.multisel) {
                g.multisel = false;
            }
        });
        $(g.gDiv).hover(function () { }, function () {
            $(g.nDiv).hide();
            $(g.nBtn).hide();
        });
        //browser adjustments
        if (browser.msie && browser.version < 7.0) {
            $('.bDiv,.mDiv,.pDiv,.vGrip,.tDiv, .sDiv', g.gDiv).css({
                width: '100%'
            });
            $(g.gDiv).addClass('ie6');
            if (p.width != 'auto') {
                $(g.gDiv).addClass('ie6fullwidthbug');
            }
        }
        //make grid functions accessible
        t.p = p;
        t.grid = g;

        $(t).attr("type", "grid");
        // load data
        if (p.url && p.autoload) {
            g.populate();
        }
        return t;
    };
    var docloaded = false;
    $(document).ready(function () {
        docloaded = true;
    });
    $.fn.flexigrid = function (p) {
        return this.each(function () {
            if (!docloaded) {
                $(this).hide();
                var t = this;
                $(document).ready(function () {
                    $.addFlex(t, p);
                });
            } else {
                $.addFlex(this, p);
            }
        });
    }; //end flexigrid
    $.fn.flexReload = function (p) { // function to reload grid
        this.each(function () {
            this.p.selectedItem = [];
        })

        return this.each(function () {
            if (this.grid && this.p.url) this.grid.populate();
        });
    }; //end flexReload
    $.fn.flexOptions = function (p) { //function to update general options
        return this.each(function () {
            if (this.grid) $.extend(this.p, p);
        });
    }; //end flexOptions    
    $.fn.flexAddData = function (data) { // function to add data to grid
        return this.each(function () {
            if (this.grid) this.grid.addData(data);
        });
    };
    $.fn.noSelect = function (p) { //no select plugin by me :-)
        var prevent = (p === null) ? true : p;
        if (prevent) {
            return this.each(function () {
                if (browser.msie || browser.safari) $(this).bind('selectstart', function () {
                    return false;
                });
                else if (browser.mozilla) {
                    $(this).css('MozUserSelect', 'none');
                    $('body').trigger('focus');
                } else if (browser.opera) $(this).bind('mousedown', function () {
                    return false;
                });
                else $(this).attr('unselectable', 'on');
            });
        } else {
            return this.each(function () {
                if (browser.msie || browser.safari) $(this).unbind('selectstart');
                else if (browser.mozilla) $(this).css('MozUserSelect', 'inherit');
                else if (browser.opera) $(this).unbind('mousedown');
                else $(this).removeAttr('unselectable', 'on');
            });
        }
    }; //end noSelect    
    $.fn.selectedItem = function (p) {  // 更改了selectedItem的获取方式，by Choey Zhao 2014-04-28
        var array = [];

        this.each(function () {
            if (this.p) {
                array = this.p.selectedItem;
            }
        })

        return array;

        //return selectedItem;
    };
    $.fn.removeData = function () { // 更改了selectedItem的清除方式，by Choey Zhao 2014-04-28
        this.each(function () {
            this.p.selectedItem = [];
        })

        return this.each(function () {
            //selectedItem = [];
            if (this.grid) this.grid.addData();
        });
    };
    $.fn.selectedRows = function (p) { // Returns the selected rows as an array, taken and adapted from http://stackoverflow.com/questions/11868404/flexigrid-get-selected-row-columns-values
        var arReturn = [];
        var arRow = [];
        var selector = $(this.selector + ' .trSelected');


        $(selector).each(function (i, row) {
            arRow = [];
            var idr = $(row).data('id');
            $.each(row.cells, function (c, cell) {
                var col = cell.abbr;
                var val = cell.firstChild.innerHTML;
                if (val == '&nbsp;') val = ''; // Trim the content
                var idx = cell.cellIndex;

                arRow.push({
                    Column: col, // Column identifier
                    Value: val, // Column value
                    CellIndex: idx, // Cell index
                    RowIdentifier: idr // Identifier of this row element
                });
            });
            arReturn.push(arRow);
        });
        return arReturn;
    };
})(jQuery);

