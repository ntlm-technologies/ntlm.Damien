# Todo

- Placer une copie de release dans chaque repository.
- Cancel des admin ne marche pas : thread ?

# Info

## Clonage

- Un `Client` représente un client comme LGI, BellesLettres, etc. 
- Les dépôts sont nommés ainsi : {client}.{projet}
- Les équipes sont nommées ainsi : {client}.dev ou {client}.admin.
- Après la validation du token, l'application découvre les équipes auxquelles appartient l'utilisateur.
- Ces équipes ont une liste de dépôts. Ainsi l'application à l'utilisateur propose pour le clonage les clients de ces dépôts.

## Équipes

- Cette fonctionnalité permet de distribuer les dépôts dans chaque équipe.
- Elle n'est disponible que pour les utilisateurs appartenant à une équipe ntlm.
- On place un utilisateur dans une équipe via l'interface Github.
- L'équipe ntlm.dev a la permission écriture sur tous les dépôts.
- L'équipe ntlm.admin a la permission admin sur tous les dépôts.
- L'équipe {client}.dev a la permission écriture sur tous les dépôts {client}.{projet}.
- L'équipe {client}.admin a la permission écriture sur tous les dépôts {client}.{projet}.
- Un `Client` a une propriété `ExtraRepositories`. S'il existe un fichier https://github.com/ntlm-technologies/ntlm.Damien.Data/{Client}.txt cette propriété sera peuplée avec tous ses noms de repositories.
- Les équipes {client}.dev et {client}.admin ont la permission lecture sur tous les dépôts de `ExtraRepositories`.

Il s'agit donc de relancer la routine quand de nouveaux dépôts sont créés.

Il suffit de créer des équipes {client}.dev ou {client}.admin puis de relancer la routine pour peupler de nouvelles équipes.  

# Publish

- Publier ntlm.Damien.Data.
- Zipper C:\Deploy\ntlm\ntlm.Damien

## Obsolète

Pour publier un unique .exe : 
dotnet publish -c Release -r win-x64 --self-contained

# Token

mambojoel
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

mambojoel2
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