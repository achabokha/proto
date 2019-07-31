import { ITheme } from "./interfaces.theme";
import { EmbilyTheme } from "./embily.theme";
import { EAlliedTheme } from "./e-allied.theme";
import { SandboxTheme } from "./sandbox.theme";
import { FilixTheme } from "./filix.theme";

export class Themes {
    public static list: ITheme[] = [
        new EmbilyTheme(),
        new EAlliedTheme(),
        new FilixTheme(),
        new SandboxTheme()
    ]
}
