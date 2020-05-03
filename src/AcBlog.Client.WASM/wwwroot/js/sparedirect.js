// Fork From https://github.com/rafrex/spa-github-pages
// Copyright (c) 2016 Rafael Pedicini, licensed under the MIT License

function sparedirectEncode(loc, segmentCount) {
    return loc.protocol + '//' + loc.hostname + (loc.port ? ':' + loc.port : '') +
        loc.pathname.split('/').slice(0, 1 + segmentCount).join('/') + '/?route=/' +
        loc.pathname.slice(1).split('/').slice(segmentCount).join('/').replace(/&/g, '~and~') +
        (loc.search ? '&query=' + loc.search.slice(1).replace(/&/g, '~and~') : '') +
        loc.hash;
}

function sparedirectDecode(loc) {
    if (loc.search) {
        var qs = {};
        loc.search.slice(1).split('&').forEach(function (v) {
            var a = v.split('=');
            qs[a[0]] = a.slice(1).join('=').replace(/~and~/g, '&');
        });
        if (qs.route !== undefined) {
            return loc.pathname.slice(0, -1) + (qs.route || '') +
                (qs.query ? ('?' + qs.query) : '') +
                loc.hash;
        }
    }
    return null;
}