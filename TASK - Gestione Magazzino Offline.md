# Esercizio — Gestione Magazzino Offline

Realizzare una semplice applicazione console per la gestione di prodotti di magazzino utilizzando una **Clean Architecture**.

La solution deve contenere:

```text
Inventory.Core
Inventory.Application
Inventory.Infrastructure
Inventory.ConsoleApp
```

La persistenza deve essere **in-memory**.

## Entità Product

Ogni prodotto contiene almeno:

```text
Id
Name
Quantity
MinimumStock
```

Regole:

- `Name` obbligatorio, minimo 3 caratteri.
- `Quantity` non può essere negativa.
- `MinimumStock` non può essere negativo.
- Un prodotto è sotto scorta quando `Quantity < MinimumStock`.

## Use Case 1 — RegisterProduct

Registrare un nuovo prodotto.

Input:

```text
Name
InitialQuantity
MinimumStock
```

Il sistema deve:

- impedire prodotti con lo stesso nome;
- creare il prodotto;
- salvarlo;
- restituire un DTO.

## Use Case 2 — RemoveStock

Prelevare una quantità di prodotto dal magazzino.

Input:

```text
ProductId
Quantity
```

Regole:

- la quantità richiesta deve essere maggiore di zero;
- non è possibile prelevare più della quantità disponibile.

La modifica della quantità deve essere gestita dal dominio e non direttamente dall'handler.

## Use Case 3 — GetLowStockProducts

Restituire tutti i prodotti per cui:

```text
Quantity < MinimumStock
```

Il risultato deve essere una collezione di DTO.

## Vincoli architetturali

- `Core` non dipende da nessun altro progetto.
- `Application` contiene casi d'uso, DTO e port.
- `Infrastructure` implementa il repository in-memory.
- `ConsoleApp` costituisce il Composition Root.
- Gli handler dipendono da un'interfaccia e non da `InMemoryProductRepository`.
- `Core` e `Application` non devono utilizzare `Console`.
- Le operazioni asincrone del repository devono propagare `CancellationToken`.

## Obiettivo

Al termine deve essere possibile sostituire:

```text
InMemoryProductRepository
```

con un futuro:

```text
SqliteProductRepository
```

senza modificare il dominio e i casi d'uso.