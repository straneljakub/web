#include <stdio.h>
#include <stdlib.h>

enum COLOR {RED, BLACK};

struct node {
	int key;
	int null;
	struct node* left;
	struct node* right;
	struct node* parent;
	enum COLOR color;
};

void PrintRB(struct node* root);
void PrintRBHelp(struct node* root, int space);
struct node* NewNode();
struct node* TreeInsert(struct node* T, struct node* x, struct node* prev);
struct node* RBInsert(struct node* T, struct node* x);
struct node* RBInsertFixup(struct node* T, struct node* x);
struct node* RotateL(struct node* T, struct node* x);
struct node* RotateR(struct node* T, struct node* x);
struct node* TreeDelete(struct node* T, struct node* x);
struct node* OrderlySuccessor(struct node* x);
struct node* Find(struct node* T, int k);
struct node* RBDelete(struct node* T, int k);
struct node* RBDeleteFixup(struct node* T, struct node* x);

int main() {

	struct node* T = NULL;


	
	T = RBInsert(T, 1);
	T = RBInsert(T, 3);
	T = RBInsert(T, 5);
	T = RBInsert(T, 7);
	T = RBInsert(T, 9);
	T = RBInsert(T, 11);


	T = RBDelete(T, 3);
	T = RBDelete(T, 5);
	T = RBDelete(T, 1);
	T = RBDelete(T, 9);
	T = RBDelete(T, 11);
	T = RBDelete(T, 7);
	
	
	
		/*
	T = RBInsert(T, 7);
	T = RBInsert(T, 6);
	T = RBInsert(T, 5);
	T = RBInsert(T, 3);
	T = RBInsert(T, 4);

	T = RBDelete(T, 4);
	T = RBDelete(T, 6);
	T = RBDelete(T, 5);
	T = RBDelete(T, 3);
	T = RBDelete(T, 7);
	*/
	


}

void PrintRBHelp(struct node* root, int space)
{
	if (root == NULL) {
		printf("EMPTY RB TREE");
		return;
	}
		
	if (root->null == 1) 
		return;
	
		

	space += 10;

	PrintRBHelp(root->right, space);


	char c;
	if (root->color == RED)
		c = 'R';
	else
		c = 'B';
	printf("\n");
	for (int i = 10; i < space; i++)
		printf(" ");
	printf("|%i %c|\n", root->key, c);
	


	PrintRBHelp(root->left, space);
}


void PrintRB(struct Node* root)
{
	PrintRBHelp(root, 0);
	printf("\n");
	for (int i = 0; i < 40; i++)
		printf("-");
	printf("\n");
}

struct node* NewNode()
{
	struct node* x = malloc(sizeof(struct node));
	x->key = 0;
	x->null = 0;
	x->left = x->right = NULL;
	x->parent = NULL;
	x->color = BLACK;
	return x;
}

struct node* TreeInsert(struct node* T, struct node* x, struct node* prev) {
	if (T->null == 1) {
		x->parent = prev;
		struct node* l = NewNode();
		l->null = 1;
		l->parent = x;
		struct node* r = NewNode();
		r->null = 1;
		r->parent = x;
		x->left = l;
		x->right = r;
		return x;
	}
		

	if (x->key < T->key)
		T->left = TreeInsert(T->left, x, T);
	else
		T->right = TreeInsert(T->right, x, T);

	return T;
}


struct node* RBInsert(struct node* T, int k) {
	if (T == NULL) {
		struct node* x = NewNode();
		x->key = k;
		x->color = BLACK;
		x->parent = x;
		struct node* l = NewNode();
		l->null = 1;
		l->parent = x;
		struct node* r = NewNode();
		r->null = 1;
		r->parent = x;
		x->left = l;
		x->right = r;
		T = x;
		printf("Inserting %i\n", k);
		PrintRB(T);
		return T;
	}
	struct node* x = NewNode();
	x->key = k;
	x->color = RED;
	T = TreeInsert(T, x, NULL);
	T = RBInsertFixup(T, x);
	printf("Inserting %i\n", k);
	PrintRB(T);
	return T;
}

struct node* RBInsertFixup(struct node* T, struct node* x) {
	while (x->parent->color == RED) {
		if (x->color == RED && x == T)
			break;
		struct node* gp = x->parent->parent;
		if (x->parent == gp->left) {
			struct node* y = gp->right;
			if (y != NULL && y->color == RED) {
				x->parent->color = BLACK;
				y->color = BLACK;
				gp->color = RED;
				x = gp;
			}
			else {
				if (x == x->parent->right) {
					x = x->parent;
					T = RotateL(T, x);
				}
				x->parent->color = BLACK;
				gp->color = RED;
				T = RotateR(T, gp);
			}
		}
		else {
			struct node* y = gp->left;
			if (y != NULL && y->color == RED) {
				x->parent->color = BLACK;
				y->color = BLACK;
				gp->color = RED;
				x = gp;
			}
			else {
				if (x == x->parent->left) {
					x = x->parent;
					T = RotateR(T, x);
				}
				x->parent->color = BLACK;
				gp->color = RED;
				T = RotateL(T, gp);
			}
		}
	}
	T->color = BLACK;
	return T;
}


struct node* RotateL(struct node* T, struct node* x) {
	struct node* right = x->right;

	if(right != NULL)
		x->right = right->left;

	if (x->right)
		x->right->parent = x;

	if (right != NULL)
		right->parent = x->parent;

	if (x->parent == x && right != NULL) {
		right->parent = right;
		T = right;
	}
	else if (x == x->parent->left)
		x->parent->left = right;
	else
		x->parent->right = right;
	right->left = x;
	x->parent = right;

	return T;
}

struct node* RotateR(struct node* T, struct node* x) {
	struct node* left = x->left;

	if (left != NULL)
		x->left = left->right;

	if (x->left)
		x->left->parent = x;

	if (left != NULL)
		left->parent = x->parent;
	if (x->parent == x && left != NULL) {
		left->parent = left;
		T = left;
	}
		
	else if (x == x->parent->left)
		x->parent->left = left;
	else
		x->parent->right = left;
	left->right = x;
	x->parent = left;
	
	return T;
}

struct node* TreeDelete(struct node* T, struct node* x, struct node* prev) {
	if (T->null == 1) 
		return T;
	
	if (x->key < T->key)
		T->left = TreeDelete(T->left, x, T);
	else if(x->key > T->key)
		T->right = TreeDelete(T->right, x, T);
	else {
		if (T->right->null == 1 && T->left->null == 1) {
			struct node* z = NewNode();
			z->null = 1;
			z->parent = prev;
			return z;
		}
		else if (T->right->null == 0)
			return T->right;
		else
			return T->left;
	}

	return T;
}

struct node* OrderlySuccessor(struct node* x) {
	struct node* y = x->right;
	while (y->left->null == 0)
		y = y->left;
	return y;
}

struct node* Find(struct node* T, int k) {
	if (T->key == k)
		return T;
	else if (k < T->key)
		Find(T->left, k);
	else
		Find(T->right, k);
}

struct node* RBDelete(struct node* T, int k) {
	struct node* z = NULL;
	enum COLOR color;
	struct node* x = Find(T, k);
	if (!(x->left->null == 0 && x->right->null == 0)) {
		color = x->color;
		T = TreeDelete(T, x, NULL);
		if (T->null == 1) {
			printf("Deleting %i\n", k);
			PrintRB(NULL);
			return NULL;
		}
			

		if (x->left->null == 0) {
			z = x->left;
			z->parent = x->parent;
		}
		else if (x->right->null == 0) {
			z = x->right;
			z->parent = x->parent;
		}
		else {
			z = x->left;
			z->parent = x->parent;
		}
		if (x->parent->right->null == 0)
			x->parent->left = z;
		else
			x->parent->right = z;
	}
	else {
		struct node* y = OrderlySuccessor(x);
		color = y->color;
		T = TreeDelete(T, y, NULL);
		if (y->left->null == 0) {
			z = y->left;
			z->parent = y->parent;
		}
		else if (y->right->null == 0) {
			z = y->right;
			z->parent = y->parent;
		}
		else {
			z = y->left;
			z->parent = y->parent;
		}
		if (y->parent->right->null == 0)
			y->parent->left = z;
		else
			y->parent->right = z;
		x->key = y->key;
	}
	if (color == BLACK) {
		T = RBDeleteFixup(T, z);
		printf("Deleting %i\n", k);
		PrintRB(T);
		return T;
	}
	else {
		printf("Deleting %i\n", k);
		PrintRB(T);
		return T;
	}
	
}

struct node* RBDeleteFixup(struct node* T, struct node* x) {
	while (x != x->parent && x->color == BLACK) {
		if (x == x->parent->left) {
			struct node* w = x->parent->right;
			if (w->color == RED) {
				w->color = BLACK;
				x->parent->color = RED;
				T = RotateL(T, x->parent);
				w = x->parent->right;
			}
			if (w->left->color == BLACK && w->right->color == BLACK) {
				w->color = RED;
				x = x->parent;
			}
			else if (w->right->color == BLACK) {
				w->left->color = BLACK;
				w->color = RED;
				T = RotateR(T, w);
				w = x->parent->right;
			}
			else {
				w->color = x->parent->color;
				x->parent->color = BLACK;
				w->right->color = BLACK;
				T = RotateL(T, x->parent);
				x = T;
			}
		}
			
		else {
			struct node* w = x->parent->left;
			if (w->color = RED) {
				w->color = BLACK;
				x->parent->color = RED;
				T = RotateR(T, x->parent);
				w = x->parent->left;
			}
			if (w->right->color == BLACK && w->left->color == BLACK) {
				w->color = RED;
				x = x->parent;
			}
			else if (w->left->color == BLACK) {
				w->right->color = BLACK;
				w->color = RED;
				T = RotateL(T, w);
				w = x->parent->left;
			}
			else {
				w->color = x->parent->color;
				x->parent->color = BLACK;
				w->left->color = BLACK;
				T = RotateR(T, x->parent);
				x = T;
			}
		}
	}
	x->color = BLACK;
	x->parent = x;
	return T;
}