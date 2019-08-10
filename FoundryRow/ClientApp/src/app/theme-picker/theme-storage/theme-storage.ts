import { Injectable, EventEmitter } from "@angular/core";

@Injectable()
export class ThemeStorage {
    static storageKey = "theme-storage-current-name";
    static defaultTheme = "purple-green";

    storeTheme(theme: string) {
        window!.localStorage[ThemeStorage.storageKey] = theme;
    }

    getStoredThemeName(): string | null {
        return window!.localStorage[ThemeStorage.storageKey] || ThemeStorage.defaultTheme;
    }

    clearStorage() {
        window!.localStorage.removeItem(ThemeStorage.storageKey);
    }
}
