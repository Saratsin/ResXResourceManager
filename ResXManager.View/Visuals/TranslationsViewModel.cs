﻿namespace tomenglertde.ResXManager.View.Visuals
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel.Composition;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Windows.Input;

    using tomenglertde.ResXManager.Infrastructure;
    using tomenglertde.ResXManager.Model;
    using tomenglertde.ResXManager.Translators;
    using tomenglertde.ResXManager.View.Properties;

    using TomsToolbox.Core;
    using TomsToolbox.Desktop;
    using TomsToolbox.ObservableCollections;
    using TomsToolbox.Wpf;
    using TomsToolbox.Wpf.Composition;

    using Settings = tomenglertde.ResXManager.Model.Properties.Settings;

    [VisualCompositionExport(RegionId.Content, Sequence = 2)]
    internal class TranslationsViewModel : ObservableObject
    {
        private readonly TranslatorHost _translatorHost;
        private readonly ResourceManager _resourceManager;
        private readonly ResourceViewModel _resourceViewModel;
        private readonly Configuration _configuration;

        private readonly ObservableCollection<TranslationItem> _selectedItems = new ObservableCollection<TranslationItem>();
        private readonly ObservableCollection<CultureKey> _selectedTargetCultures = new ObservableCollection<CultureKey>();

        private CultureKey _sourceCulture;
        private ICollection<TranslationItem> _items = new TranslationItem[0];
        private ITranslationSession _translationSession;
        private ICollection<CultureKey> _allTargetCultures = new CultureKey[0];


        [ImportingConstructor]
        public TranslationsViewModel(TranslatorHost translatorHost, ResourceManager resourceManager, ResourceViewModel resourceViewModel, Configuration configuration)
        {
            Contract.Requires(translatorHost != null);
            Contract.Requires(resourceManager != null);
            Contract.Requires(resourceViewModel != null);
            Contract.Requires(configuration != null);

            _translatorHost = translatorHost;
            _resourceManager = resourceManager;
            _resourceViewModel = resourceViewModel;
            _configuration = configuration;

            _resourceManager.Loaded += ResourceManager_Loaded;

            SourceCulture = _resourceManager.Cultures.FirstOrDefault();

            _selectedTargetCultures.CollectionChanged += SelectedTargetCultures_CollectionChanged;
        }

        public ResourceManager ResourceManager => _resourceManager;

        public Configuration Configuration => _configuration;

        public IEnumerable<ITranslator> Translators => _translatorHost.Translators;

        public CultureKey SourceCulture
        {
            get
            {
                return _sourceCulture;
            }
            set
            {
                if (SetProperty(ref _sourceCulture, value, () => SourceCulture))
                {
                    AllTargetCultures = _resourceManager
                        .Cultures
                        .ObservableWhere(key => key != _sourceCulture);
                }
            }
        }

        public ICollection<CultureKey> AllTargetCultures
        {
            get
            {
                Contract.Ensures(Contract.Result<ICollection<CultureKey>>() != null);

                return _allTargetCultures;
            }
            private set
            {
                Contract.Requires(value != null);

                if (SetProperty(ref _allTargetCultures, value, nameof(AllTargetCultures)))
                {
                    _selectedTargetCultures.SynchronizeWith(value.Except(UnselectedTargetCultures).ToArray());
                }
            }
        }

        public ICollection<CultureKey> SelectedTargetCultures
        {
            get
            {
                Contract.Ensures(Contract.Result<ICollection<CultureKey>>() != null);

                return _selectedTargetCultures;
            }
        }

        public ICollection<TranslationItem> Items
        {
            get
            {
                Contract.Ensures(Contract.Result<ICollection<TranslationItem>>() != null);

                return _items;
            }
            private set
            {
                Contract.Requires(value != null);

                SetProperty(ref _items, value, () => Items);
            }
        }

        public ICollection<TranslationItem> SelectedItems
        {
            get
            {
                Contract.Ensures(Contract.Result<ICollection<TranslationItem>>() != null);

                return _selectedItems;
            }
        }

        public ITranslationSession TranslationSession
        {
            get
            {
                return _translationSession;
            }
            set
            {
                SetProperty(ref _translationSession, value, () => TranslationSession);
            }
        }

        public ICommand StartCommand
        {
            get
            {
                Contract.Ensures(Contract.Result<ICommand>() != null);

                return new DelegateCommand(() => _translationSession == null, UpdateTargetList);
            }
        }

        public ICommand RestartCommand
        {
            get
            {
                Contract.Ensures(Contract.Result<ICommand>() != null);

                return new DelegateCommand(() => SourceCulture != null, UpdateTargetList);
            }
        }

        public ICommand ApplyAllCommand
        {
            get
            {
                Contract.Ensures(Contract.Result<ICommand>() != null);

                return new DelegateCommand(() => IsSessionComplete && Items.Any(), () => Apply(Items));
            }
        }

        public ICommand ApplySelectedCommand
        {
            get
            {
                Contract.Ensures(Contract.Result<ICommand>() != null);

                return new DelegateCommand(() => IsSessionComplete && SelectedItems.Any(), () => Apply(SelectedItems));
            }
        }

        public ICommand StopCommand
        {
            get
            {
                Contract.Ensures(Contract.Result<ICommand>() != null);

                return new DelegateCommand(() => IsSessionRunning, Stop);
            }
        }

        private void ResourceManager_Loaded(object sender, EventArgs e)
        {
            if ((SourceCulture == null) || !_resourceManager.Cultures.Contains(SourceCulture))
                SourceCulture = _resourceManager.Cultures.FirstOrDefault();

            Items = new TranslationItem[0];
        }

        private void Stop()
        {
            _translationSession?.Cancel();
        }

        private void Apply(IEnumerable<TranslationItem> items)
        {
            Contract.Requires(items != null);

            var prefix = _configuration.EffectiveTranslationPrefix;

            foreach (var item in items.Where(item => !string.IsNullOrEmpty(item.Translation)).ToArray())
            {
                Contract.Assume(item != null);

                var entry = item.Entry;

                if (!entry.CanEdit(item.TargetCulture))
                    break;

                entry.Values.SetValue(item.TargetCulture, prefix + item.Translation);
                Items.Remove(item);
            }
        }

        private bool IsSessionComplete => _translationSession != null && _translationSession.IsComplete;

        private bool IsSessionRunning => _translationSession != null && !_translationSession.IsComplete && !_translationSession.IsCanceled;

        private static IEnumerable<CultureKey> UnselectedTargetCultures
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<CultureKey>>() != null);

                return (Settings.Default.TranslationUnselectedTargetCultures ?? string.Empty).Split(',').Select(c => c.ToCultureKey()).Where(c => c != null);
            }
            set
            {
                Contract.Requires(value != null);

                Settings.Default.TranslationUnselectedTargetCultures = string.Join(",", value);
            }
        }

        private void UpdateTargetList()
        {
            TranslationSession?.Cancel();

            SelectedItems.Clear();

            if (_sourceCulture == null)
            {
                Items = new TranslationItem[0];
                return;
            }

            Items = GetItemsToTranslate(_resourceViewModel.ResourceTableEntries, _sourceCulture, _configuration.EffectiveTranslationPrefix, _selectedTargetCultures);

            ApplyExistingTranslations(_resourceViewModel.ResourceTableEntries, Items, _sourceCulture);

            TranslationSession = new TranslationSession(_sourceCulture.Culture, _configuration.NeutralResourcesLanguage, Items.Cast<ITranslationItem>().ToArray());

            _translatorHost.Translate(TranslationSession);
        }

        private void SelectedTargetCultures_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UnselectedTargetCultures = _allTargetCultures.Concat(UnselectedTargetCultures).Distinct().Except(_selectedTargetCultures);
        }

        private static ICollection<TranslationItem> GetItemsToTranslate(IEnumerable<ResourceTableEntry> resourceTableEntries, CultureKey sourceCulture, string translationPrefix, IEnumerable<CultureKey> targetCultures)
        {
            Contract.Requires(resourceTableEntries != null);
            Contract.Requires(sourceCulture != null);
            Contract.Requires(targetCultures != null);
            Contract.Ensures(Contract.Result<ICollection<TranslationItem>>() != null);

            return new ObservableCollection<TranslationItem>(
                targetCultures.SelectMany(targetCulture =>
                    resourceTableEntries
                        .Where(entry => !entry.IsInvariant)
                        .Select(entry => new { Entry = entry, Source = entry.Values.GetValue(sourceCulture), Target = entry.Values.GetValue(targetCulture) })
                        .Where(item => string.IsNullOrWhiteSpace(item.Target) || string.Equals(item.Target, translationPrefix, System.StringComparison.Ordinal))
                        .Where(item => !string.IsNullOrWhiteSpace(item.Source))
                        .Select(item => new TranslationItem(item.Entry, item.Source, targetCulture))));
        }

        private static void ApplyExistingTranslations(ICollection<ResourceTableEntry> resourceTableEntries, IEnumerable<TranslationItem> items, CultureKey sourceCulture)
        {
            Contract.Requires(resourceTableEntries != null);
            Contract.Requires(items != null);
            Contract.Requires(sourceCulture != null);

            foreach (var item in items)
            {
                var targetItem = item;
                Contract.Assume(targetItem != null);
                var targetCulture = targetItem.TargetCulture;

                var existingTranslations = resourceTableEntries
                    .Where(entry => entry != targetItem.Entry)
                    .Where(entry => !entry.IsInvariant)
                    .Where(entry => entry.Values.GetValue(sourceCulture) == targetItem.Source)
                    .Select(entry => entry.Values.GetValue(targetCulture))
                    .Where(translation => !string.IsNullOrWhiteSpace(translation))
                    .GroupBy(translation => translation);

                foreach (var translation in existingTranslations)
                {
                    Contract.Assume(translation != null);
                    item.Results.Add(new TranslationMatch(null, translation.Key, translation.Count()));
                }
            }
        }

        public override string ToString() => Resources.ShellTabHeader_Translate;

        [ContractInvariantMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
        private void ObjectInvariant()
        {
            Contract.Invariant(_translatorHost != null);
            Contract.Invariant(_resourceManager != null);
            Contract.Invariant(_resourceViewModel != null);
            Contract.Invariant(_configuration != null);
            Contract.Invariant(_selectedItems != null);
            Contract.Invariant(_selectedTargetCultures != null);
            Contract.Invariant(_items != null);
            Contract.Invariant(_allTargetCultures != null);
        }
    }
}
