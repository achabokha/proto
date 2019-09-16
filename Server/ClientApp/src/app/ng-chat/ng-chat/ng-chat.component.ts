import { Component, Input, OnInit, ViewChildren, ViewChild, HostListener, Output, EventEmitter, ElementRef, ViewEncapsulation } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { ChatAdapter } from "./core/chat-adapter";
import { IChatGroupAdapter } from "./core/chat-group-adapter";
import { User } from "./core/user";
import { ParticipantResponse } from "./core/participant-response";
import { Message } from "./core/message";
import { FileMessage } from "./core/file-message";
import { MessageType } from "./core/message-type.enum";
import { Window } from "./core/window";
import { ChatParticipantStatus } from "./core/chat-participant-status.enum";
import { ScrollDirection } from "./core/scroll-direction.enum";
import { Localization, StatusDescription } from "./core/localization";
import { IChatController } from "./core/chat-controller";
import { PagedHistoryChatAdapter } from "./core/paged-history-chat-adapter";
import { IFileUploadAdapter } from "./core/file-upload-adapter";
import { DefaultFileUploadAdapter } from "./core/default-file-upload-adapter";
import { Theme } from "./core/theme.enum";
import { IChatOption } from "./core/chat-option";
import { Group } from "./core/group";
import { ChatParticipantType } from "./core/chat-participant-type.enum";
import { IChatParticipant } from "./core/chat-participant";

import { map } from "rxjs/operators";
import { Observable } from "rxjs";
import { MatSidenav } from "@angular/material";

@Component({
    selector: 'ng-chat',
    templateUrl: "ng-chat.component.html",
    styleUrls: [
        "assets/icons.css",
        "assets/loading-spinner.css",
        "assets/ng-chat.component.default.css",
        "assets/themes/ng-chat.theme.default.scss",
        "assets/themes/ng-chat.theme.dark.scss"
    ],
    encapsulation: ViewEncapsulation.None
})

export class NgChat implements OnInit, IChatController {


    constructor(private _httpClient: HttpClient) { }

    get isDisabled(): boolean {
        return this._isDisabled;
    }

    @Input()
    set isDisabled(value: boolean) {
        this._isDisabled = value;

        if (value) {
            // To address issue https://github.com/rpaschoal/ng-chat/issues/120
            window.clearInterval(this.pollingIntervalWindowInstance);
        } else {
            this.activateFriendListFetch();
        }
    }

    private get localStorageKey(): string {
        return `ng-chat-users-${this.userId}`; // Appending the user id so the state is unique per user in a computer.
    }

    get filteredParticipants(): Group[] {
        if (this.searchInput.length > 0) {
            // Searches in the friend list by the inputted search string
            return this.participants.filter(x => x.displayName.toUpperCase().includes(this.searchInput.toUpperCase()));
        }

        return this.participants;
    }

    @ViewChild("sidenav", { static: false }) sideNav: MatSidenav;

    // Exposes enums for the ng-template
    public ChatParticipantType = ChatParticipantType;
    public ChatParticipantStatus = ChatParticipantStatus;
    public MessageType = MessageType;

    private _isDisabled = false;

    @Input()
    public adapter: ChatAdapter;

    @Input()
    public groupAdapter: IChatGroupAdapter;

    @Input()
    public userId: any;

    @Input()
    public userEmail: any;

    @Input()
    public isCollapsed = false;

    @Input()
    public maximizeWindowOnNewMessage = true;

    @Input()
    public pollFriendsList = false;

    @Input()
    public pollingInterval = 5000;

    @Input()
    public historyEnabled = true;

    @Input()
    public emojisEnabled = true;

    @Input()
    public linkfyEnabled = true;

    @Input()
    public audioEnabled = true;

    @Input()
    public searchEnabled = true;

    @Input() // TODO: This might need a better content strategy
    public audioSource = "http://localhost:5000/assets/sounds/chat_alertshort.mp3";

    @Input()
    public persistWindowsState = true;

    @Input()
    public title = "Users online";

    @Input()
    public messagePlaceholder = "Type a message";

    @Input()
    public searchPlaceholder = "Search";

    @Input()
    public browserNotificationsEnabled = true;

    @Input() // TODO: This might need a better content strategy
    public browserNotificationIconSource = "https://raw.githubusercontent.com/rpaschoal/ng-chat/master/src/ng-chat/assets/notification.png";

    @Input()
    public browserNotificationTitle = "New message from";

    @Input()
    public historyPageSize = 10;

    @Input()
    public localization: Localization;

    @Input()
    public hideFriendsList = false;

    @Input()
    public hideFriendsListOnUnsupportedViewport = true;

    @Input()
    public fileUploadUrl: string;

    @Input()
    public theme: Theme = Theme.Light;

    @Input()
    public customTheme: string;

    @Input()
    public messageDatePipeFormat = "short";

    @Input()
    public showMessageDate = true;

    @Input()
    public isViewportOnMobileEnabled = false;

    @Output()
    public onParticipantClicked: EventEmitter<Group> = new EventEmitter<Group>();

    @Output()
    public onParticipantChatOpened: EventEmitter<Group> = new EventEmitter<Group>();

    @Output()
    public onParticipantChatClosed: EventEmitter<Group> = new EventEmitter<Group>();

    @Output()
    public onMessagesSeen: EventEmitter<Message[]> = new EventEmitter<Message[]>();

    private browserNotificationsBootstrapped = false;

    public hasPagedHistory = false;

    // Don't want to add this as a setting to simplify usage. Previous placeholder and title settings available to be used, or use full Localization object.
    private statusDescription: StatusDescription = {
        online: "Online",
        busy: "Busy",
        away: "Away",
        offline: "Offline"
    };

    private audioFile: HTMLAudioElement;

    public searchInput = "";

    protected participants: Group[];

    protected participantsResponse: ParticipantResponse[];

    private participantsInteractedWith: Group[] = [];

    public currentActiveOption: IChatOption | null;

    protected selectedUsersFromFriendsList: Group[] = [];

    private pollingIntervalWindowInstance: number;

    // Defines the size of each opened window to calculate how many windows can be opened on the viewport at the same time.
    public windowSizeFactor = 320;

    // Total width size of the friends list section
    public friendsListWidth = 262;

    // Available area to render the plugin
    private viewPortTotalArea: number;

    // Set to true if there is no space to display at least one chat window and 'hideFriendsListOnUnsupportedViewport' is true
    public unsupportedViewport = false;

    // File upload state
    public fileUploadersInUse: string[] = []; // Id bucket of uploaders in use
    public fileUploadAdapter: IFileUploadAdapter;

    windows: Window[] = [];

    isBootstrapped = false;

    @ViewChildren("chatMessages") chatMessageClusters: any;

    @ViewChildren("chatWindowInput") chatWindowInputs: any;

    @ViewChildren("nativeFileInput") nativeFileInputs: ElementRef[];

    public defaultWindowOptions(currentWindow: Window): IChatOption[] {
        // if (this.groupAdapter && currentWindow.participant.participantType == ChatParticipantType.User) {
        //     return [{
        //         isActive: false,
        //         action: (chattingWindow: Window) => {
        //             this.sideNav.open();
        //             this.selectedUsersFromFriendsList = this.selectedUsersFromFriendsList.concat(chattingWindow.participant as User);
        //         },
        //         validateContext: (participant: IChatParticipant) => {
        //             return participant.participantType == ChatParticipantType.User;
        //         },
        //         displayLabel: "Add People" // TODO: Localize this
        //     }];
        // }

        return [];
    }

    ngOnInit() {
        this.bootstrapChat();
    }

    @HostListener("window:resize", ["$event"])
    onResize(event: any) {
        this.viewPortTotalArea = event.target.innerWidth;

        this.NormalizeWindows();
    }

    // Checks if there are more opened windows than the view port can display
    private NormalizeWindows(): void {
        const maxSupportedOpenedWindows = Math.floor((this.viewPortTotalArea - (!this.hideFriendsList ? this.friendsListWidth : 0)) / this.windowSizeFactor);
        const difference = this.windows.length - maxSupportedOpenedWindows;

        if (difference >= 0) {
            this.windows.splice(this.windows.length - difference);
        }

        this.updateWindowsState(this.windows);

        // Viewport should have space for at least one chat window but should show in mobile if option is enabled.
        this.unsupportedViewport = this.isViewportOnMobileEnabled ? false : this.hideFriendsListOnUnsupportedViewport && maxSupportedOpenedWindows < 1;
    }

    // Initializes the chat plugin and the messaging adapter
    private bootstrapChat(): void {
        let initializationException = null;

        if (this.adapter != null && this.userId != null) {
            try {
                this.viewPortTotalArea = window.innerWidth;

                this.initializeTheme();
                this.initializeDefaultText();
                this.initializeBrowserNotifications();

                // Binding event listeners
                this.adapter.messageReceivedHandler = (msg) => this.onMessageReceived(msg);
                this.adapter.friendsListChangedHandler = (participantsResponse) => this.onFriendsListChanged(participantsResponse);

                this.activateFriendListFetch();

                this.bufferAudioFile();

                this.hasPagedHistory = this.adapter instanceof PagedHistoryChatAdapter;

                if (this.fileUploadUrl && this.fileUploadUrl !== "") {
                    this.fileUploadAdapter = new DefaultFileUploadAdapter(this.fileUploadUrl, this._httpClient);
                }

                this.NormalizeWindows();

                this.isBootstrapped = true;
            } catch (ex) {
                initializationException = ex;
            }
        }

        if (!this.isBootstrapped) {
            console.error("ng-chat component couldn't be bootstrapped.");

            if (this.userId == null) {
                console.error("ng-chat can't be initialized without an user id. Please make sure you've provided an userId as a parameter of the ng-chat component.");
            }
            if (this.adapter == null) {
                console.error("ng-chat can't be bootstrapped without a ChatAdapter. Please make sure you've provided a ChatAdapter implementation as a parameter of the ng-chat component.");
            }
            if (initializationException) {
                console.error(`An exception has occurred while initializing ng-chat. Details: ${initializationException.message}`);
                console.error(initializationException);
            }
        }
    }

    private activateFriendListFetch(): void {
        if (this.adapter) {
            // Loading current users list
            if (this.pollFriendsList) {
                // Setting a long poll interval to update the friends list
                this.fetchFriendsList(true);
                //this.pollingIntervalWindowInstance = window.setInterval(() => this.fetchFriendsList(false), this.pollingInterval);
            } else {
                // Since polling was disabled, a friends list update mechanism will have to be implemented in the ChatAdapter.
                this.fetchFriendsList(true);
            }
        }
    }

    // Initializes browser notifications
    private async initializeBrowserNotifications() {
        if (this.browserNotificationsEnabled && ("Notification" in window)) {
            if (await Notification.requestPermission()) {
                this.browserNotificationsBootstrapped = true;
            }
        }
    }

    // Initializes default text
    private initializeDefaultText(): void {
        if (!this.localization) {
            this.localization = {
                messagePlaceholder: this.messagePlaceholder,
                searchPlaceholder: this.searchPlaceholder,
                title: this.title,
                statusDescription: this.statusDescription,
                browserNotificationTitle: this.browserNotificationTitle,
                loadMessageHistoryPlaceholder: "Load older messages"
            };
        }
    }

    private initializeTheme(): void {
        if (this.customTheme) {
            this.theme = Theme.Custom;
        } else if (this.theme != Theme.Light && this.theme != Theme.Dark) {
            // TODO: Use es2017 in future with Object.values(Theme).includes(this.theme) to do this check
            throw new Error(`Invalid theme configuration for ng-chat. "${this.theme}" is not a valid theme value.`);
        }
    }

    // Sends a request to load the friends list
    private fetchFriendsList(isBootstrapping: boolean): void {
        this.adapter.userList("")
            .pipe(
                map((participantsResponse: ParticipantResponse[]) => {
                    this.participantsResponse = participantsResponse;

                    this.participants = participantsResponse.map((response: ParticipantResponse) => {
                        const g = new Group(response.participants);
                        g.groupId = response.metadata.groupId;
                        g.status = response.participants[0].status;
                        g.unreadMessages = response.metadata.totalUnreadMessages;
                        return g;
                    });
                })
            ).subscribe(() => {
                if (isBootstrapping) {
                    this.restoreWindowsState();
                }
            });
    }

    fetchMessageHistory(window: Window) {
        // Not ideal but will keep this until we decide if we are shipping pagination with the default adapter
        if (this.adapter instanceof PagedHistoryChatAdapter) {
            window.isLoadingHistory = true;

            this.adapter.getMessageHistoryByPage(window.participant.hubContextId, this.historyPageSize, ++window.historyPage)
                .pipe(
                    map((result: Message[]) => {
                        result.forEach((message) => this.assertMessageType(message));

                        window.messages = result.concat(window.messages);
                        window.isLoadingHistory = false;

                        const direction: ScrollDirection = (window.historyPage == 1) ? ScrollDirection.Bottom : ScrollDirection.Top;
                        window.hasMoreMessages = result.length == this.historyPageSize;

                        setTimeout(() => this.onFetchMessageHistoryLoaded(result, window, direction, true));
                    })
                ).subscribe();
        } else {
            this.adapter.getGroupMessageHistory(window.participant.groupId)
                .pipe(
                    map((result: Message[]) => {
                        result.forEach((message) => this.assertMessageType(message));

                        window.messages = result.concat(window.messages);
                        window.isLoadingHistory = false;

                        setTimeout(() => this.onFetchMessageHistoryLoaded(result, window, ScrollDirection.Bottom));
                    })
                ).subscribe();

        }
    }

    private onFetchMessageHistoryLoaded(messages: Message[], window: Window, direction: ScrollDirection, forceMarkMessagesAsSeen: boolean = false): void {
        this.scrollChatWindow(window, direction);

        if (window.hasFocus || forceMarkMessagesAsSeen) {
            const unseenMessages = messages.filter(m => !m.dateSeen);

            this.markMessagesAsRead(unseenMessages);
            this.onMessagesSeen.emit(unseenMessages);
        }
    }

    // Updates the friends list via the event handler
    private onFriendsListChanged(participant: IChatParticipant): void {
        if (participant) {
            this.participants = this.participants.map(m => {
                const ind = m.chattingTo.findIndex(d => d.userId === participant.userId);
                if (ind > -1) {
                    m.chattingTo[ind].status = participant.status;
                    if(m.chattingTo.length == 2) {
                        m.status = participant.status;
                    }
                }
                return m;
            });

            this.participantsInteractedWith = [];
        }
    }

    // Handles received messages by the adapter
    private async onMessageReceived(message: Message) {
        if (message) {
            let participant = new Group([message.fromUser]);
            if (this.windows != null) {
                let existingWindow = this.windows.find(d => d.participant.groupId === message.groupId);
                if (existingWindow) {
                    participant = existingWindow.participant;
                } else {
                    participant.groupId = message.groupId;
                }
            }

            this.openChatWindow(participant).then(chatWindow => {

                this.assertMessageType(message);

                if (!chatWindow[1] || !this.historyEnabled) {
                    chatWindow[0].messages.push(message);

                    this.scrollChatWindow(chatWindow[0], ScrollDirection.Bottom);

                    if (chatWindow[0].hasFocus) {
                        this.markMessagesAsRead([message]);
                        this.onMessagesSeen.emit([message]);
                    }
                }

                this.emitMessageSound(chatWindow[0]);

                // Github issue #58
                // Do not push browser notifications with message content for privacy purposes if the 'maximizeWindowOnNewMessage' setting is off and this is a new chat window.
                if (this.maximizeWindowOnNewMessage || (!chatWindow[1] && !chatWindow[0].isCollapsed)) {
                    // Some messages are not pushed because they are loaded by fetching the history hence why we supply the message here
                    this.emitBrowserNotification(chatWindow[0], message);
                }
            });
        }
    }

    public togleSidePanel(cancel: boolean): Promise<any> {
        if(cancel) {
            return new Promise((resolve) => resolve({}));
        } else {
        if (this.sideNav.opened) {
            return this.sideNav.close();
        } else {
            return this.sideNav.open();
        }
    }
    }

    // Opens a new chat whindow. Takes care of available viewport
    // Works for opening a chat window for an user or for a group
    // Returns => [Window: Window object reference, boolean: Indicates if this window is a new chat window]
    public openChatWindow(participants: Group, focusOnNewWindow: boolean = false, invokedByUserClick: boolean = false): Promise<[Window, boolean]> {
        // Is this window opened?
        const openedWindow = this.windows.find(x => x.participant.groupId === participants.groupId);

        return new Promise((resolve, reject) => {
            this.togleSidePanel(!invokedByUserClick).then(() => {
                if (!openedWindow) {
                    if (invokedByUserClick) {
                        this.onParticipantClicked.emit(participants);
                    }
    
                    // Refer to issue #58 on Github
                    const collapseWindow = invokedByUserClick ? false : !this.maximizeWindowOnNewMessage;
    
                    const newChatWindow: Window = new Window(participants, this.historyEnabled, collapseWindow);
    
                    this.adapter.createNewGroup(participants).subscribe(chatRoomId => {
    
                        participants.groupId = chatRoomId;
                        if (this.groupAdapter) {
                            this.groupAdapter.groupCreated(participants);
                        }
    
                        // Loads the chat history via an RxJs Observable
                        if (this.historyEnabled) {
                            this.fetchMessageHistory(newChatWindow);
                        }
    
                        this.windows = [newChatWindow];
    
                        this.updateWindowsState(this.windows);
    
                        if (focusOnNewWindow && !collapseWindow) {
                            this.focusOnWindow(newChatWindow);
                        }
    
                        this.participantsInteractedWith.push(participants);
                        this.onParticipantChatOpened.emit(participants);
    
                        resolve([newChatWindow, true]);
                    });
    
    
                } else {
                    // Returns the existing chat window
                    resolve([openedWindow, false]);
                }
            });
        });
        


    }

    // Focus on the input element of the supplied window
    private focusOnWindow(window: Window, callback: Function = () => { }): void {
        const windowIndex = this.windows.indexOf(window);
        if (windowIndex >= 0) {
            setTimeout(() => {
                if (this.chatWindowInputs) {
                    const messageInputToFocus = this.chatWindowInputs.toArray()[windowIndex];

                    messageInputToFocus.nativeElement.focus();
                }

                callback();
            });
        }
    }

    // Scrolls a chat window message flow to the bottom
    private scrollChatWindow(window: Window, direction: ScrollDirection): void {
        if (!window.isCollapsed) {
            const windowIndex = this.windows.indexOf(window);
            setTimeout(() => {
                if (this.chatMessageClusters) {
                    const targetWindow = this.chatMessageClusters.toArray()[windowIndex];

                    if (targetWindow) {
                        const element = this.chatMessageClusters.toArray()[windowIndex].nativeElement;
                        const position = (direction === ScrollDirection.Top) ? 0 : element.scrollHeight;
                        element.scrollTop = position;
                    }
                }
            });
        }
    }

    // Marks all messages provided as read with the current time.
    public markMessagesAsRead(messages: Message[]): void {
        const currentDate = new Date();

        messages.forEach((msg) => {
            msg.dateSeen = currentDate;
        });
    }

    // Buffers audio file (For component's bootstrapping)
    private bufferAudioFile(): void {
        if (this.audioSource && this.audioSource.length > 0) {
            this.audioFile = new Audio();
            this.audioFile.src = this.audioSource;
            this.audioFile.load();
        }
    }

    // Emits a message notification audio if enabled after every message received
    private emitMessageSound(window: Window): void {
        if (this.audioEnabled && !window.hasFocus && this.audioFile) {
            this.audioFile.play();
        }
    }

    // Emits a browser notification
    private emitBrowserNotification(window: Window, message: Message): void {
        if (this.browserNotificationsBootstrapped && !window.hasFocus && message) {
            const notification = new Notification(`${this.localization.browserNotificationTitle} ${window.participant.displayName}`, {
                "body": message.message,
                "icon": this.browserNotificationIconSource
            });

            setTimeout(() => {
                notification.close();
            }, message.message.length <= 50 ? 5000 : 7000); // More time to read longer messages
        }
    }

    // Saves current windows state into local storage if persistence is enabled
    private updateWindowsState(windows: Window[]): void {
        if (this.persistWindowsState) {
            const participantIds = windows.map((w) => {
                return w.participant.hubContextId;
            });

            localStorage.setItem(this.localStorageKey, JSON.stringify(participantIds));
        }
    }

    private restoreWindowsState(): void {
        try {
            if (this.persistWindowsState) {
                const stringfiedParticipantIds = localStorage.getItem(this.localStorageKey);

                if (stringfiedParticipantIds && stringfiedParticipantIds.length > 0) {
                    const participantIds = JSON.parse(stringfiedParticipantIds) as string[];

                    const participantsToRestore = this.participants.filter(u => participantIds.indexOf(u.groupId) >= 0);

                    participantsToRestore.forEach((participant) => {
                        this.openChatWindow(participant);
                    });
                }
            }
        } catch (ex) {
            console.error(`An error occurred while restoring ng-chat windows state. Details: ${ex}`);
        }
    }

    // Gets closest open window if any. Most recent opened has priority (Right)
    private getClosestWindow(window: Window): Window | undefined {
        const index = this.windows.indexOf(window);

        if (index > 0) {
            return this.windows[index - 1];
        } else if (index == 0 && this.windows.length > 1) {
            return this.windows[index + 1];
        }
    }

    private assertMessageType(message: Message): void {
        // Always fallback to "Text" messages to avoid rendenring issues
        if (!message.type) {
            message.type = MessageType.Text;
        }
    }

    private formatUnreadMessagesTotal(totalUnreadMessages: number): string {
        if (totalUnreadMessages > 0) {

            if (totalUnreadMessages > 99) {
                return "99+";
            }
            else {
                return String(totalUnreadMessages);
            }
        }

        // Empty fallback.
        return "";
    }

    // Returns the total unread messages from a chat window. TODO: Could use some Angular pipes in the future
    unreadMessagesTotal(window: Window): string {
        let totalUnreadMessages = 0;

        if (window) {
            totalUnreadMessages = window.messages.filter(x => x.fromUser.userId != this.adapter.authService.currentUser.id && !x.dateSeen).length;
        }

        return this.formatUnreadMessagesTotal(totalUnreadMessages);
    }

    unreadMessagesTotalByParticipant(participant: Group): string {
        const openedWindow = this.windows.find(x => x.participant.groupId === participant.groupId);

        if (openedWindow) {
            return this.unreadMessagesTotal(openedWindow);
        } else {
            const totalUnreadMessages = this.participantsResponse
                .filter(x => x.metadata.groupId == participant.groupId 
                        && !this.participantsInteractedWith.find(u => u.groupId == participant.groupId) && x.metadata && x.metadata.totalUnreadMessages > 0)
                .map((participantResponse) => {
                    return participantResponse.metadata.totalUnreadMessages;
                })[0];

            return this.formatUnreadMessagesTotal(totalUnreadMessages);
        }
    }

    /*  Monitors pressed keys on a chat window
        - Dispatches a message when the ENTER key is pressed
        - Tabs between windows on TAB or SHIFT + TAB
        - Closes the current focused window on ESC
    */
    onChatInputTyped(event: any, window: Window): void {
        switch (event.keyCode) {
            case 13:
                if (window.newMessage && window.newMessage.trim() != "") {
                    const message = new Message();

                    message.fromUser = {
                        avatar: null,
                        displayName: this.adapter.authService.currentUser.email,
                        userId: this.adapter.authService.currentUser.id,
                        hubContextId: this.userId,
                        participantType: ChatParticipantType.User,
                        status: ChatParticipantStatus.Online
                    };

                    message.message = window.newMessage;
                    message.dateSent = new Date();
                    message.groupId = window.participant.groupId;


                    window.messages.push(message);

                    this.adapter.sendMessage(message);

                    window.newMessage = ""; // Resets the new message input

                    this.scrollChatWindow(window, ScrollDirection.Bottom);
                }
                break;
            case 9:
                event.preventDefault();

                const currentWindowIndex = this.windows.indexOf(window);
                let messageInputToFocus = this.chatWindowInputs.toArray()[currentWindowIndex + (event.shiftKey ? 1 : -1)]; // Goes back on shift + tab

                if (!messageInputToFocus) {
                    // Edge windows, go to start or end
                    messageInputToFocus = this.chatWindowInputs.toArray()[currentWindowIndex > 0 ? 0 : this.chatWindowInputs.length - 1];
                }

                messageInputToFocus.nativeElement.focus();

                break;
            case 27:
                const closestWindow = this.getClosestWindow(window);

                if (closestWindow) {
                    this.focusOnWindow(closestWindow, () => { this.onCloseChatWindow(window); });
                } else {
                    this.onCloseChatWindow(window);
                }
        }
    }

    // Closes a chat window via the close 'X' button
    onCloseChatWindow(window: Window): void {
        const index = this.windows.indexOf(window);

        this.windows.splice(index, 1);

        this.updateWindowsState(this.windows);

        this.onParticipantChatClosed.emit(window.participant);
    }

    // Toggle friends list visibility
    onChatTitleClicked(event: any): void {
        this.isCollapsed = !this.isCollapsed;
    }

    // Toggles a chat window visibility between maximized/minimized
    onChatWindowClicked(window: Window): void {
        window.isCollapsed = !window.isCollapsed;
        this.scrollChatWindow(window, ScrollDirection.Bottom);
    }

    // Asserts if a user avatar is visible in a chat cluster
    isAvatarVisible(window: Window, message: Message, index: number): boolean {
        if (this.isReceivedChatMessage(window, message)) {
            if (index == 0) {
                return true; // First message, good to show the thumbnail
            } else {
                // Check if the previous message belongs to the same user, if it belongs there is no need to show the avatar again to form the message cluster
                if (window.messages[index - 1].fromUser.userId != message.fromUser.userId) {
                    return true;
                }
            }
        }

        return false;
    }

    // Asserts is message is received from user or sent by user
    isSentChatMessage(window: Window, message: Message) {
        if (message.fromUser) {
            return message.fromUser.userId === this.adapter.authService.currentUser.id;
        } else {
            return false;
        }
    }

    // Asserts if message is received by window participant
    isReceivedChatMessage(window: Window, message: Message) {
        if (this.adapter.authService.currentUser.id !== message.fromUser.userId) {
            return true;
        } else {
            return false;
        }
    }

    getChatWindowAvatar(participant: Group, message: Message): string | null {
        const userIndex = participant.chattingTo.findIndex(x => x.userId == message.fromUser.userId);

        return participant.chattingTo[userIndex >= 0 ? userIndex : 0].avatar;
    }

    // Toggles a window focus on the focus/blur of a 'newMessage' input
    toggleWindowFocus(window: Window): void {
        window.hasFocus = !window.hasFocus;
        if (window.hasFocus) {
            const unreadMessages = window.messages
                .filter(message => message.dateSeen == null
                    && (message.fromUser.userId == this.adapter.authService.currentUser.id));

            if (unreadMessages && unreadMessages.length > 0) {
                this.markMessagesAsRead(unreadMessages);
                this.onMessagesSeen.emit(unreadMessages);
            }
        }
    }

    // [Localized] Returns the status descriptive title
    getStatusTitle(status: ChatParticipantStatus): any {
        const currentStatus = status.toString().toLowerCase();

        return this.localization.statusDescription[currentStatus];
    }

    triggerOpenChatWindow(participants: Group): void {
        if (participants) {
            this.openChatWindow(participants);
        }
    }

    triggerCloseChatWindow(groupId: any): void {
        const openedWindow = this.windows.find(x => x.participant.groupId == groupId);

        if (openedWindow) {
            this.onCloseChatWindow(openedWindow);
        }
    }

    triggerToggleChatWindowVisibility(groupId: any): void {
        const openedWindow = this.windows.find(x => x.participant.groupId == groupId);

        if (openedWindow) {
            this.onChatWindowClicked(openedWindow);
        }
    }

    // Generates a unique file uploader id for each participant
    getUniqueFileUploadInstanceId(window: Window): string {
        if (window && window.participant) {
            return `ng-chat-file-upload-${window.participant.groupId}`;
        }

        return "ng-chat-file-upload";
    }

    // Triggers native file upload for file selection from the user
    triggerNativeFileUpload(window: Window): void {
        if (window) {
            const fileUploadInstanceId = this.getUniqueFileUploadInstanceId(window);
            const uploadElementRef = this.nativeFileInputs.filter(x => x.nativeElement.id === fileUploadInstanceId)[0];

            if (uploadElementRef) {
                uploadElementRef.nativeElement.click();
            }
        }
    }

    private clearInUseFileUploader(fileUploadInstanceId: string): void {
        const uploaderInstanceIdIndex = this.fileUploadersInUse.indexOf(fileUploadInstanceId);

        if (uploaderInstanceIdIndex > -1) {
            this.fileUploadersInUse.splice(uploaderInstanceIdIndex, 1);
        }
    }

    isUploadingFile(window: Window): boolean {
        const fileUploadInstanceId = this.getUniqueFileUploadInstanceId(window);

        return this.fileUploadersInUse.indexOf(fileUploadInstanceId) > -1;
    }

    // Handles file selection and uploads the selected file using the file upload adapter
    onFileChosen(window: Window): void {
        const fileUploadInstanceId = this.getUniqueFileUploadInstanceId(window);
        const uploadElementRef = this.nativeFileInputs.filter(x => x.nativeElement.id === fileUploadInstanceId)[0];

        if (uploadElementRef) {
            const file: File = uploadElementRef.nativeElement.files[0];

            this.fileUploadersInUse.push(fileUploadInstanceId);

            this.fileUploadAdapter.uploadFile(file, window.participant.groupId)
                .subscribe(fileMessage => {
                    this.clearInUseFileUploader(fileUploadInstanceId);

                    //fileMessage.fromUser.userId = this.adapter.authService.currentUser.id;

                    // Push file message to current user window
                    window.messages.push(fileMessage);

                    this.adapter.sendMessage(fileMessage);

                    this.scrollChatWindow(window, ScrollDirection.Bottom);

                    // Resets the file upload element
                    uploadElementRef.nativeElement.value = "";
                }, (error) => {
                    this.clearInUseFileUploader(fileUploadInstanceId);

                    // Resets the file upload element
                    uploadElementRef.nativeElement.value = "";

                    // TODO: Invoke a file upload adapter error here
                });
        }
    }

    onFriendsListCheckboxChange(selectedUser: Group, isChecked: boolean): void {
        if (isChecked) {
            this.selectedUsersFromFriendsList.push(selectedUser);
        } else {
            this.selectedUsersFromFriendsList.splice(this.selectedUsersFromFriendsList.indexOf(selectedUser), 1);
        }
    }

    onFriendsListActionCancelClicked(): void {
        if (this.currentActiveOption) {
            this.currentActiveOption.isActive = false;
            this.currentActiveOption = null;
            this.selectedUsersFromFriendsList = [];
        }
    }

    onFriendsListActionConfirmClicked(): void {
        let chattingTo: IChatParticipant[];
        this.selectedUsersFromFriendsList.forEach(element => {
            chattingTo.concat(element.chattingTo);
        });
        const newGroup = new Group(chattingTo);

        this.openChatWindow(newGroup);

        // Canceling current state
        this.onFriendsListActionCancelClicked();
    }

    isUserSelectedFromFriendsList(group: Group): boolean {
        return (this.selectedUsersFromFriendsList.filter(item => item.groupId == group.groupId)).length > 0;
    }
}
