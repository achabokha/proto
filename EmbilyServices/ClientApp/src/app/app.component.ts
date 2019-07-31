import { Component, OnInit, Inject } from '@angular/core';
import { DOCUMENT } from '@angular/platform-browser';
import { ITheme, Themes } from './themes';
import { DataService } from './services/data.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
    title = 'EmbilyServices';

    constructor(@Inject(DOCUMENT) private document: any, private data: DataService) {
    }

    ngOnInit(): void {
        let domain = this.document.location.hostname.replace('wwww.', '');
        if (domain == 'localhost')
            domain = 'embilyservices2.azurewebsites.net';
        //domain = 'services.embily.com';
        //domain = 'sandbox.embily.com';
        //domain = 'e-allied.com';

        console.log('domain:', domain);

        let theme: ITheme = this.getThemeByDomain(domain);

        let themeName = this.getQueryVariable('theme');
        if (themeName) {
            theme = this.getThemeByName(themeName);
        }

        this.setTheme(theme);
    }

    getThemeByDomain(domain: string): ITheme {
        return Themes.list.find((t) => t.domain == domain);
    }

    getThemeByName(name: string): ITheme {
        return Themes.list.find((t) => t.name == name);
    }

    setTheme(theme: ITheme) {
        this.data.theme = theme;

        let kvpairs = Object.entries(theme.styles);

        let t = this.document.querySelector('body');
        kvpairs.forEach((kv) => {
            t.style.setProperty(kv[0], kv[1]);
        });
    }

    getQueryVariable(variable) {
        var query = this.document.location.search.substring(1);
        var vars = query.split('&');
        for (var i = 0; i < vars.length; i++) {
            var pair = vars[i].split('=');
            if (decodeURIComponent(pair[0]) == variable) {
                return decodeURIComponent(pair[1]);
            }
        }
        console.log('Query variable %s not found', variable);
        return null;

    }
}
