import { Component, OnInit, Output, EventEmitter } from "@angular/core";
import { FlatTreeControl } from "@angular/cdk/tree";
import { Observable, of as observableOf } from "rxjs";
import { MatTreeFlattener, MatTreeFlatDataSource } from "@angular/material/tree";
import { CommentsService, TreeNode } from "src/app/services/comments.service";

/** Flat node with expandable and level information */
interface FlatNode {
    expandable: boolean;
    level: number;
    isExpanded?: boolean;
    id: string;
    parentId: string;
    name: string;
    content: string;
    datetime: string;
    likes: number;
    dislikes: number;
    loves: number;
}

@Component({
    selector: "app-comments",
    templateUrl: "./comments.component.html",
    styleUrls: ["./comments.component.scss"]
})
export class CommentsComponent implements OnInit {
    @Output() onTreeNodeChanged = new EventEmitter();

    private _getLevel = (node: FlatNode) => node.level;

    private _isExpandable = (node: FlatNode) => node.expandable;

    private _getChildren = (node: TreeNode): Observable<TreeNode[]> => observableOf(node.children);

    hasChild = (_: number, node: FlatNode) => node.expandable;

    isLoadMore = (index: number, node: FlatNode) => index > 3;

    private transformer = (node: TreeNode, level: number): FlatNode => {
        return {
            expandable: node.children.length > 0,
            level: level,
            id: node.id,
            parentId: node.parentId,
            name: node.name,
            content: node.content,
            dislikes: node.dislikes,
            likes: node.likes,
            loves: node.likes,
            datetime: node.datetime
        };
    };

    expandedNodeSet = new Set<string>();

    private treeFlattener: MatTreeFlattener<TreeNode, FlatNode> = new MatTreeFlattener(
        this.transformer,
        this._getLevel,
        this._isExpandable,
        this._getChildren
    );

    treeControl = new FlatTreeControl<FlatNode>(this._getLevel, this._isExpandable);
    dataSource = new MatTreeFlatDataSource(this.treeControl, this.treeFlattener);

    selectedNode: FlatNode;

    isSelected = (node: FlatNode) => this.selectedNode && node.id == this.selectedNode.id;

    constructor(private commentsService: CommentsService) {}

    ngOnInit() {
        this.commentsService.getCommentTree().subscribe((result: any) => {
            this.dataSource.data = result;
            this.treeControl.expandAll();
        });
    }

    selectNode(node: FlatNode) {
        this.onTreeNodeChanged.emit(node);
        this.selectedNode = node;
    }

    loadMore(node: FlatNode) {

    } 

    edit(node) {
        // save edit here --
    }

    delete(node) {
        this.rememberExpandedTreeNodes(this.treeControl, this.expandedNodeSet);

        // console.log("looking for: ", node.parentId);

        let parentNode = this.findTreeNodeById(this.dataSource.data[0], node.parentId);

        // console.log('found: ', parentNode);

        parentNode.children = parentNode.children.filter(n => n.id != node.id);

        this.dataSource.data = this.dataSource.data;
        this.expandNodesById(this.treeControl.dataNodes, Array.from(this.expandedNodeSet));
    }

    private rememberExpandedTreeNodes(treeControl: FlatTreeControl<FlatNode>, expandedNodeSet: Set<string>) {
        if (treeControl.dataNodes) {
            treeControl.dataNodes.forEach(node => {
                if (treeControl.isExpandable(node) && treeControl.isExpanded(node)) {
                    // capture latest expanded state
                    expandedNodeSet.add(node.id);
                }
            });
        }
    }

    private expandNodesById(flatNodes: FlatNode[], ids: string[]) {
        if (!flatNodes || flatNodes.length === 0) return;
        const idSet = new Set(ids);
        return flatNodes.forEach(node => {
            if (idSet.has(node.id)) {
                this.treeControl.expand(node);
                let parent = this.getParentNode(node);
                while (parent) {
                    this.treeControl.expand(parent);
                    parent = this.getParentNode(parent);
                }
            }
        });
    }

    private getParentNode(node: FlatNode): FlatNode | null {
        const currentLevel = node.level;
        if (currentLevel < 1) {
            return null;
        }
        const startIndex = this.treeControl.dataNodes.indexOf(node) - 1;
        for (let i = startIndex; i >= 0; i--) {
            const currentNode = this.treeControl.dataNodes[i];
            if (currentNode.level < currentLevel) {
                return currentNode;
            }
        }
        return null;
    }

    // todo: removal --
    findTreeNodeById(node: TreeNode, id: string): TreeNode {
        // console.log("name: ", node.name, "id: ", node.id, "#ch: ", node.children.length, "looking for: ", id);

        if(node.parentId == null) return node;

        for (var i = 0; node.children && i < node.children.length; i++) {
            const n = node.children[i];
            if (n.id == id) {
                console.log("found! ", n);
                return n;
            }
            var found = this.findTreeNodeById(n, id);
            if (found) return found;
        }

        return null;
    }
}
