import { Component, ViewEncapsulation, ChangeDetectionStrategy, NgModule, OnInit, OnDestroy, Output, EventEmitter, Input } from "@angular/core";
import { MatButtonModule } from "@angular/material/button";
import { MatGridListModule } from "@angular/material/grid-list";
import { MatIconModule } from "@angular/material/icon";
import { MatMenuModule } from "@angular/material/menu";
import { MatTooltipModule } from "@angular/material/tooltip";
import { CommonModule } from "@angular/common";
import { ActivatedRoute } from "@angular/router";
import { Subscription } from "rxjs";
import { map, filter } from "rxjs/operators";
import { OverlayContainer } from "@angular/cdk/overlay";
import { ThemeStorage } from "./theme-storage/theme-storage";

interface SiteTheme {
    name: string;
    accent: string;
    primary: string;
}

@Component({
    selector: "theme-picker",
    templateUrl: "theme-picker.html",
    styleUrls: ["theme-picker.scss"],
    changeDetection: ChangeDetectionStrategy.OnPush,
    encapsulation: ViewEncapsulation.None,
    host: { "aria-hidden": "true" }
})
export class ThemePicker implements OnInit, OnDestroy {
    private queryParamSubscription = Subscription.EMPTY;
    currentTheme: SiteTheme;

    themes: SiteTheme[] = [
        {
            primary: "#673AB7",
            accent: "#FFC107",
            name: "deeppurple-amber",
        },
        {
            primary: "#3F51B5",
            accent: "#E91E63",
            name: "indigo-pink",
        },
        {
            primary: "#E91E63",
            accent: "#607D8B",
            name: "pink-bluegrey",
        },
        {
            primary: "#9C27B0",
            accent: "#4CAF50",
            name: "purple-green",
        }
    ];

    @Input() initThemeName;
    @Output() onThemeChanged = new EventEmitter<string>();

    constructor(private themeStorage: ThemeStorage, private activatedRoute: ActivatedRoute, public overlayContainer: OverlayContainer) {
        this.changeTheme(this.themeStorage.getStoredThemeName());
    }

    ngOnInit() {
        this.queryParamSubscription = this.activatedRoute.queryParamMap
            .pipe(
                map(params => params.get("theme")),
                filter(Boolean)
            )
            .subscribe(themeName => this.changeTheme(themeName));
    }

    ngOnDestroy() {
        this.queryParamSubscription.unsubscribe();
    }

    changeTheme(themeName: string) {
        const theme = this.themes.find(currentTheme => currentTheme.name === themeName);
        if (!theme) return;

        this.currentTheme = theme;
        this.themeStorage.storeTheme(this.currentTheme.name);

        this.onThemeChanged.emit(themeName);
    }
}

@NgModule({
    imports: [MatButtonModule, MatIconModule, MatMenuModule, MatGridListModule, MatTooltipModule, CommonModule],
    exports: [ThemePicker],
    declarations: [ThemePicker],
    providers: [ThemeStorage]
})
export class ThemePickerModule {}
